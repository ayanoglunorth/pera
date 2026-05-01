using System;

namespace Pera.DTO.DTOs
{
    // 1. Mesaj Göndermek İçin
    public class MesajGonderDto
    {
        public string AliciId { get; set; }
        public string Icerik { get; set; }
    }

    // 2. Sohbet Balonları İçin (Detay)
    public class MesajDetayDto
    {
        public int Id { get; set; }
        public string GondericiId { get; set; }
        public string GondericiAd { get; set; }
        public string Icerik { get; set; }
        public DateTime Tarih { get; set; }
        public bool BenMiGonderdim { get; set; } // Sağda mı solda mı duracak?
        public bool OkunduMu { get; set; }
    }

    // 3. Sol Menüdeki Liste İçin (Inbox)
    public class SohbetKutusuDto
    {
        public string KullaniciId { get; set; } // Sohbet ettiğim kişinin ID'si
        public string AdSoyad { get; set; }     // Sohbet ettiğim kişinin Adı
        public string SonMesaj { get; set; }    // "Görüşürüz..."
        public DateTime SonTarih { get; set; }  // 10:42
        public int OkunmamisSayisi { get; set; } // Mavi yuvarlak içindeki sayı
    }
}