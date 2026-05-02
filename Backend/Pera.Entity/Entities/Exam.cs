using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pera.Entity.Entities
{
    [Table("Exams")]
    public class Exam
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } // Example: "345 Publications"
        public DateTime Date { get; set; }

        public string Type { get; set; }

        public int? CourseId { get; set; } // Can be null (for general exams)
        public Course Course { get; set; }
        public ICollection<ExamResult> Results { get; set; }
    }
}
