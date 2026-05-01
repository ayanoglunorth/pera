using System;

namespace Pera.DTO.DTOs
{
    public class DenemeListeDto
    {
        public int SinavId { get; set; }      
        public string Ad { get; set; }        
        public DateTime Tarih { get; set; }   
        public decimal ToplamNet { get; set; } 
        public decimal? Puan { get; set; }     
        public decimal TurkceNet { get; set; }
        public decimal MatematikNet { get; set; }
        public decimal FenNet { get; set; }
        public decimal InkilapNet { get; set; }
        public decimal DinNet { get; set; }
        public decimal IngilizceNet { get; set; }
    }
}