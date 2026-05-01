using System;
namespace Pera.DTO.DTOs
{
    // 1. ORTAK PARÇA (Hem Girişte Hem Çıkışta Kullanılır)
    public class DersSonucDto
    {
        public string DersAdi { get; set; }
        public int DogruSayisi { get; set; }
        public int YanlisSayisi { get; set; }
    }

    // 2. VERİ GİRİŞİ İÇİN (Frontend -> Backend)
    // Öğretmen "Kaydet" dediğinde bu kullanılır.
    public class DenemeSonucGirisDto
    {
        public int SinavId { get; set; }
        public string OgrenciId { get; set; }
        public List<DersSonucDto> Dersler { get; set; }
    }

    // 3. VERİ GÖSTERME İÇİN (Backend -> Frontend)
    // Öğrenci "Sonucum ne?" dediğinde bu kullanılır.
    public class DenemeDetayDto
    {
        public int SinavId { get; set; }
        public string Ad { get; set; }
        public DateTime Tarih { get; set; }
        public decimal? Puan { get; set; }
        public decimal ToplamNet { get; set; }

        public List<DersSonucDto> Dersler { get; set; }
    }
}