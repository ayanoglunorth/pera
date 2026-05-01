using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pera.DTO.DTOs
{
    public class SinavEkleDto
    {
        public string Baslik { get; set; } // Örn: "TYT Deneme 1"
        public DateTime Tarih { get; set; }
        public int DersId { get; set; }    // Hangi dersin sınavı?
    }
}
