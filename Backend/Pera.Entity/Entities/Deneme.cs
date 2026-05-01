using Pera.Entity.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Pera.Entity
{
    public class Deneme
    {
        [Key]
        public int Id { get; set; }
        public string Ad { get; set; } // Örn: "345 Yayınları gibi"
        public DateTime Tarih { get; set; }

        public string Tur { get; set; }

        public int? DersId { get; set; } // Boş olabilir (Genel Denemeler için)
        public Ders Ders { get; set; }
        public ICollection<DenemeSonuc> Sonuclar { get; set; }
    }
}