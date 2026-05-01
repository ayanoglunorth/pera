using System;

namespace Pera.Entity.Entities
{
    public class Mesaj
    {
        public int Id { get; set; }

        // Kimden?
        public string GondericiId { get; set; }
        public AppUser Gonderici { get; set; }

        // Kime?
        public string AliciId { get; set; }
        public AppUser Alici { get; set; }

        public string Icerik { get; set; }
        public DateTime Tarih { get; set; }

        // Mavi tık mantığı için
        public bool OkunduMu { get; set; } = false;
    }
}