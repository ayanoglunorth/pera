using System.Collections.Generic;

namespace Pera.DTO.DTOs
{
    // 1. Frontend'e göndereceğimiz liste (Öğrenci + Puanı)
    public class SinavOgrenciListeDto
    {
        public string OgrenciId { get; set; }
        public string AdSoyad { get; set; }
        public int? Puan { get; set; } // Null olabilir (Not girilmemişse)
    }

    // 2. Frontend'den gelen kaydetme paketi
    public class TopluSonucGirisDto
    {
        public int SinavId { get; set; }
        public List<OgrenciNotuDto> Kayitlar { get; set; }
    }

    public class OgrenciNotuDto
    {
        public string OgrenciId { get; set; }
        public int Puan { get; set; }
    }
}