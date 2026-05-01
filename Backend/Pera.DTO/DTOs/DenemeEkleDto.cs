using System;
using System.Collections.Generic;

namespace Pera.DTO.DTOs
{
    // 1. SONUÇ GİRERKEN KULLANILACAK DTO (Öğretmen not girerken)
    public class DenemeEkleDto
    {
        // ARTIK İSİM YOK! Sadece ID var.
        // Çünkü "Töder-1" sınavının adı veritabanında zaten kayıtlı.
        public int DenemeId { get; set; }

        public string OgrenciId { get; set; } // Öğrencinin ID'si (GUID string olarak)

        public List<DersEkleDto> Dersler { get; set; }
    }

    // Tablonun Her Bir Satırı (Senin DersSonucSatiri dediğin sınıf)
    // Service kodunda ismini DersEkleDto olarak kullandık, o yüzden değiştirdim.
    public class DersEkleDto
    {
        public string DersAdi { get; set; }
        public int DogruSayisi { get; set; }
        public int YanlisSayisi { get; set; }
    }

    // 2. YENİ SINAV TANIMLARKEN KULLANILACAK DTO (Sadece Admin/Öğretmen)
    // Bunu "Sınav Tanımla" sayfasında kullanacağız.
    public class DenemeTanimlaDto
    {
        public string Ad { get; set; }    // Örn: "Töder Türkiye Geneli - 1"
        public DateTime Tarih { get; set; }

        public string Tur { get; set; }

        public int? DersId { get; set; }
    }
}