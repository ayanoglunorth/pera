using Microsoft.AspNetCore.Mvc;
using Pera.Business.Abstract;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims; // DİKKAT: ID'yi bulmak için bu kütüphane şart!

namespace Pera.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Sadece giriş yapmış kullanıcılar girebilir
    public class DerslerController : ControllerBase
    {
        private readonly IDersService _dersService;

        public DerslerController(IDersService dersService)
        {
            _dersService = dersService;
        }

        [HttpGet("notlar")] // Endpoint adı: api/dersler/notlar
        public IActionResult GetDersNotlari()
        {
            // 1. ADIM: Token'dan "Ben kimim?" sorusunun cevabını (ID'yi) alıyoruz.
            // ClaimTypes.NameIdentifier genelde UserID'yi tutar.
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Eğer ID bulunamazsa (Token bozuksa) hata dön
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Kullanıcı kimliği bulunamadı.");
            }

            // 2. ADIM: ID'yi servise gönderip "Bu çocuğun notlarını getir" diyoruz.
            var data = _dersService.GetOgrenciDersNotlari(userId);

            return Ok(data);
        }
    }
}