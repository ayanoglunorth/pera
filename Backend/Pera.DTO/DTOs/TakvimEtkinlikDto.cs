using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pera.DTO.DTOs
{
    public class TakvimEtkinlikDto
    {
        public int Id { get; set; }
        public string Baslik { get; set; }   // Örn: "Matematik 1. Yazılı"
        public DateTime Tarih { get; set; }
        public string Aciklama { get; set; } 
        public string Renk { get; set; }     // Örn: "#FF5733" (Turuncu) - Frontend bu renge boyayacak
    }
}
