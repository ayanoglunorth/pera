using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Pera.Business.Abstract;
using Pera.DTO.DTOs;
using Pera.Entity; // AppUser'ın burada olduğundan emin ol
using Pera.Entity.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Pera.Business.Concrete
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        // DİKKAT: <int> kısmını kaldırdım. Standart IdentityRole genelde daha az sorun çıkarır.
        // Eğer AppUser sınıfını <int> olarak ayarlamadıysan burası IdentityRole olmalı.
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        // Dönüş tipini bool yaptım, Controller'da "if(true)" demek daha kolaydır.
        public async Task<bool> RegisterAsync(RegisterDto model)
        {
            var user = new AppUser
            {
                UserName = model.Email,
                Email = model.Email,
                Ad = model.Ad,
                Soyad = model.Soyad
                // ProfilResmi veritabanında zorunlu değilse veya varsayılan varsa buraya yazmana gerek yok.
            };

            var result = await _userManager.CreateAsync(user, model.Sifre);

            if (result.Succeeded)
            {
                // --- KRİTİK NOKTA: ROL OLUŞTURMA VE ATAMA ---

                // 1. Roller veritabanında yoksa oluştur (Atom bombası sonrası şart!)
                if (!await _roleManager.RoleExistsAsync("Ogretmen"))
                    await _roleManager.CreateAsync(new IdentityRole("Ogretmen"));

                if (!await _roleManager.RoleExistsAsync("Ogrenci"))
                    await _roleManager.CreateAsync(new IdentityRole("Ogrenci"));

                // 2. Kullanıcının seçimine göre rolü yapıştır
                if (model.OgretmenMi)
                {
                    await _userManager.AddToRoleAsync(user, "Ogretmen");
                }
                else
                {
                    await _userManager.AddToRoleAsync(user, "Ogrenci");
                }

                return true; // Kayıt ve Rol atama başarılı
            }

            // Hata varsa (Örn: Şifre yetersiz)
            return false;
        }

        public async Task<string> LoginAsync(LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null) return null;

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Sifre, false);

            // Başarılıysa Token üret, değilse null dön
            if (result.Succeeded)
            {
                return await GenerateJwtToken(user);
            }

            return null;
        }

        public async Task<List<OgrenciSecimDto>> GetOgrencilerAsync()
        {
            var users = await _userManager.GetUsersInRoleAsync("Ogrenci");

            // Listeyi yeni ismimizle oluşturuyoruz
            var ogrenciListesi = new List<OgrenciSecimDto>();

            foreach (var user in users)
            {
                ogrenciListesi.Add(new OgrenciSecimDto
                {
                    Id = user.Id,
                    Ad = user.Ad,
                    Soyad = user.Soyad
                });
            }

            return ogrenciListesi;
        }
        private async Task<string> GenerateJwtToken(AppUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email ?? ""),
                new Claim(ClaimTypes.Name, $"{user.Ad} {user.Soyad}")
            };

            
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                // 1. Standart Rol (Backend içi güvenlik için)
                claims.Add(new Claim(ClaimTypes.Role, role));

                // 2. Bizim Özel Rolümüz (Frontend kolay okusun diye) <--- YENİ HAMLE
                claims.Add(new Claim("role", role));
            }
            // ---------------------------

            var keyStr = _configuration["JwtSettings:SecurityKey"] ?? "PeraProjectSecretKey1234567890123456";
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyStr));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}