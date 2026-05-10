using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Pera.Entity.Entities;

namespace Pera.Entity.Entities
{
    [Table("Exams")]
    public class Exam
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }

        public string Type { get; set; }

        public int? CourseId { get; set; }
        public Course? Course { get; set; }
        public ICollection<ExamResult> Results { get; set; }
    }
}
