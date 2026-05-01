using Pera.Entity.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pera.Entity
{
    public class DenemeSonuc
    {
        [Key]
        public int Id { get; set; }

        // --- İLİŞKİLER ---
        // Hangi Deneme?
        public int DenemeId { get; set; }
        public Deneme Deneme { get; set; }

        // Hangi Öğrenci? (AppUser kullanıyoruz)
        public string OgrenciId { get; set; }
        [ForeignKey("OgrenciId")]
        public AppUser Ogrenci { get; set; }

        // Türkçe
        public int TurkceDogru { get; set; }
        public int TurkceYanlis { get; set; }
        public decimal TurkceNet { get; set; } // Net virgüllü olabilir (3.75 gibi)

        // Matematik
        public int MatematikDogru { get; set; }
        public int MatematikYanlis { get; set; }
        public decimal MatematikNet { get; set; }

        // Fen Bilimleri
        public int FenDogru { get; set; }
        public int FenYanlis { get; set; }
        public decimal FenNet { get; set; }

        // İnkilap
        public int InkilapDogru { get; set; }
        public int InkilapYanlis { get; set; }
        public decimal InkilapNet { get; set; }

        // Din
        public int DinDogru { get; set; }
        public int DinYanlis { get; set; }
        public decimal DinNet { get; set; }

        // İngilizce

        [Column("İngilizceDogru")]
        public int IngilizceDogru { get; set; }

        [Column("İngilizceYanlis")]
        public int IngilizceYanlis { get; set; }

        [Column("İngilizceNet")]
        public decimal IngilizceNet { get; set; }

        // --- GENEL TOPLAM ---
        public decimal ToplamNet { get; set; }
        public decimal? Puan { get; set; } // Puan hesaplanırsa buraya yazılır
    }
}