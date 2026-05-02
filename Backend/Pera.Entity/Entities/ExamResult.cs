using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pera.Entity.Entities
{
    [Table("ExamResults")]
    public class ExamResult
    {
        [Key]
        public int Id { get; set; }

        // --- RELATIONSHIPS ---
        // Which Exam?
        public int ExamId { get; set; }
        public Exam Exam { get; set; }

        // Which Student? (Using AppUser)
        public string StudentId { get; set; }
        [ForeignKey("StudentId")]
        public AppUser Student { get; set; }

        // Turkish
        public int TurkishCorrect { get; set; }
        public int TurkishWrong { get; set; }
        public decimal TurkishNet { get; set; } // Net can be decimal (e.g., 3.75)

        // Math
        public int MathCorrect { get; set; }
        public int MathWrong { get; set; }
        public decimal MathNet { get; set; }

        // Science
        public int ScienceCorrect { get; set; }
        public int ScienceWrong { get; set; }
        public decimal ScienceNet { get; set; }

        // History
        public int HistoryCorrect { get; set; }
        public int HistoryWrong { get; set; }
        public decimal HistoryNet { get; set; }

        // Religion
        public int ReligionCorrect { get; set; }
        public int ReligionWrong { get; set; }
        public decimal ReligionNet { get; set; }

        // English

        [Column("EnglishCorrect")]
        public int EnglishCorrect { get; set; }

        [Column("EnglishWrong")]
        public int EnglishWrong { get; set; }

        [Column("EnglishNet")]
        public decimal EnglishNet { get; set; }

        // --- TOTALS ---
        public decimal TotalNet { get; set; }
        public decimal? Score { get; set; } // If score is calculated, it goes here
    }
}
