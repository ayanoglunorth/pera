using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pera.Business.Abstract;
using Pera.DTO.DTOs;
using System.Security.Claims;

namespace Pera.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DenemelerController : ControllerBase
    {
        // İŞTE _denemeService BURADA TANIMLANIYOR
        private readonly IDenemeService _denemeService;

        public DenemelerController(IDenemeService denemeService)
        {
            _denemeService = denemeService;
        }

        // --- 1. SINAV TANIMLAMA ---
        [HttpPost("tanimla")]
        public IActionResult Tanimla(DenemeTanimlaDto model)
        {
            _denemeService.DenemeTanimla(model);
            return Ok(new { message = "Sınav tanımı oluşturuldu." });
        }

        // --- 2. DROPDOWN İÇİN LİSTE ---
        // --- 2. DROPDOWN İÇİN LİSTE ---
        [HttpGet("tanimlar")]
        public IActionResult GetTanimlar([FromQuery] string tur = null)
        {
            // Eğer tur gönderilirse filtreler, gönderilmezse hepsini getirir (Takvim için)
            var result = _denemeService.GetDenemeTanimlari(tur);
            return Ok(result);
        }

        // --- 3. SINAV SONUCU EKLE ---
        [HttpPost("ekle")]
        public IActionResult Ekle(DenemeEkleDto model)
        {
            if (model.Dersler == null || model.Dersler.Count == 0)
            {
                return BadRequest("Lütfen ders sonuçlarını giriniz.");
            }

            _denemeService.DenemeGirisiYap(model);
            return Ok(new { message = "Deneme sınavı ve sonuçları başarıyla kaydedildi." });
        }

        // --- 4. ÖĞRENCİ KENDİ LİSTESİNİ GÖRÜR ---
        [HttpGet("listem")]
        public IActionResult GetListem()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var result = _denemeService.GetSonuclarByOgrenci(userId);
            return Ok(result);
        }

        // --- 5. ÖĞRETMEN ÖĞRENCİ SEÇER ---
        [HttpGet("ogrenci-sonuclari/{ogrenciId}")]
        public IActionResult GetOgrenciSonuclari(string ogrenciId)
        {
            var result = _denemeService.GetSonuclarByOgrenci(ogrenciId);
            return Ok(result);
        }

        // --- 6. DETAY GÖRME ---
        [HttpGet("detay/{id}")]
        public IActionResult GetDetay(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // YENİ: Token'dan rolü de okuyoruz
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            // Servise rolü de gönderiyoruz
            var result = _denemeService.GetDenemeDetay(id, userId, userRole);

            if (result == null) return NotFound("Sınav bulunamadı veya yetkiniz yok.");
            return Ok(result);
        }

        // --- 7. SİLME ---
        [HttpDelete("sil/{id}")]
        public IActionResult Sil(int id)
        {
            try
            {
                _denemeService.DenemeSil(id);
                return Ok(new { message = "Sınav başarıyla silindi." });
            }
            catch (Exception ex)
            {
                return BadRequest("Silme işlemi başarısız: " + ex.Message);
            }
        }

        // --- 8. SINIF ORTALAMASI (YENİ EKLENEN KISIM) ---
        [HttpGet("sinif-ortalamasi")]
        public IActionResult GetSinifOrtalamasi()
        {
            // _denemeService artık burada hata vermeyecek çünkü yukarıda tanımlı
            var result = _denemeService.GetSinifOrtalamasi();
            return Ok(result);
        }

        [HttpGet("ogrenci-listesi/{sinavId}")]
        public async Task<IActionResult> GetOgrenciListesi(int sinavId)
        {
            var liste = await _denemeService.GetSinavOgrenciListesiAsync(sinavId);
            return Ok(liste);
        }

        [HttpPost("toplu-sonuc")]
        public IActionResult TopluSonucKaydet([FromBody] TopluSonucGirisDto model)
        {
            _denemeService.TopluSonucKaydet(model);
            return Ok(new { message = "Notlar başarıyla kaydedildi." });
        }

        [HttpGet("ogrenci-karnesi")]
        public IActionResult GetOgrenciKarnesi()
        {
            // Token'dan ID'yi al
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            // Servisten sadece bu öğrencinin sonuçlarını getir
            var sonuclar = _denemeService.GetSonuclarByOgrenci(userId);

            return Ok(sonuclar);
        }

        [HttpPost("sonuc-ekle")]
        public IActionResult SonucEkle([FromBody] DenemeSonucGirisDto model)
        {
            // Frontend'den gelen 'model' dolu mu diye basit bir kontrol
            if (model == null || model.Dersler == null || model.Dersler.Count == 0)
            {
                return BadRequest("Lütfen ders sonuçlarını giriniz.");
            }

            // Servise gönder
            _denemeService.DenemeSonucuEkle(model);

            return Ok(new { message = "Öğrenci sonuçları başarıyla kaydedildi." });
        }

    }
}