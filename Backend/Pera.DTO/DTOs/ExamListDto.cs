using System;

namespace Pera.DTO.DTOs
{
    public class ExamListDto
    {
        public int ExamId { get; set; }      
        public string Name { get; set; }        
        public DateTime Date { get; set; }   
        public decimal TotalNet { get; set; } 
        public decimal? Score { get; set; }     
        public decimal TurkishNet { get; set; }
        public decimal MathNet { get; set; }
        public decimal ScienceNet { get; set; }
        public decimal HistoryNet { get; set; }
        public decimal ReligionNet { get; set; }
        public decimal EnglishNet { get; set; }
    }
}
