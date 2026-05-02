using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity; // UserManager için gerekli
using Microsoft.AspNetCore.Mvc;
using Pera.Business.Abstract;
using Pera.DTO.DTOs;
using Pera.Entity.Entities; // AppUser için gerekli
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pera.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        // 1. EKLENDİ: _userManager değişkenini tanımladık
        private readonly UserManager<AppUser> _userManager;

        // 2. GÜNCELLENDİ: Constructor'a UserManager parametresi ekledik ve eşleştirdik
        public AuthController(IAuthService authService, UserManager<AppUser> userManager)
        {
            _authService = authService;
            _userManager = userManager; // Eşleştirme yapıldı
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            var result = await _authService.RegisterAsync(model);

            if (result)
            {
                return Ok(new { message = "Kayıt başarıyla oluşturuldu." });
            }

            return BadRequest(new { message = "Kayıt başarısız. Şifreniz çok basit olabilir veya bu email zaten kayıtlı." });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto model)
        {
            var token = await _authService.LoginAsync(model);

            if (token != null)
            {
                return Ok(new { token = token, message = "Giriş başarılı!" });
            }

            return Unauthorized(new { message = "Giriş başarısız. Email veya şifre hatalı." });
        }

        [HttpGet("students")]
        public async Task<IActionResult> GetStudents()
        {
            var result = await _authService.GetStudentsAsync();
            return Ok(result);
        }

        // --- YENİ EKLENEN VE DÜZELTİLEN METOT ---
        [Authorize]
        [HttpGet("chat-users")]
        public async Task<IActionResult> GetChatUsers()
        {
            // 1. Önce giriş yapan kullanıcıyı bulalım
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var currentUser = await _userManager.FindByIdAsync(userId);

            // 2. Kullanıcının rolünü DB'den kesin olarak çekelim
            var roles = await _userManager.GetRolesAsync(currentUser);
            bool isTeacher = roles.Contains("Teacher");
            bool isStudent = roles.Contains("Student");

            List<AppUser> targetUsers = new List<AppUser>();

            if (isTeacher)
            {
                // SEN ÖĞRETMENSİN -> KARŞINA ÖĞRENCİLER GELSİN
                var students = await _userManager.GetUsersInRoleAsync("Student");
                targetUsers = students.ToList();
            }
            else if (isStudent)
            {
                // SEN ÖĞRENCİSİN -> KARŞINA ÖĞRETMENLER GELSİN
                // (Dikkat: Burası 'Ogretmen' olmalı)
                var teachers = await _userManager.GetUsersInRoleAsync("Teacher");
                targetUsers = teachers.ToList();
            }

            // 3. Listeden kendimizi çıkaralım (Hata payını sıfıra indirmek için)
            targetUsers = targetUsers.Where(u => u.Id != userId).ToList();

            // 4. Sonuçları DTO formatında döndürelim
            var result = targetUsers.Select(u => new
            {
                Id = u.Id,
                FirstName = u.FirstName ?? u.UserName,
                LastName = u.LastName ?? "",
                Role = isTeacher ? "Student" : "Teacher"
            }).ToList();

            return Ok(result);
        }
    }
}