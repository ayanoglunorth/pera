using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pera.Entity.Entities
{
    [Table("Courses")]
    public class Course
    {
        // Using standard 'Id' instead of old 'CourseId'
        public int Id { get; set; }

        public string Name { get; set; }

        // Now connects to 'Exam' table instead of 'Exam'
        // Because school written exams are also stored in Exam table (Type="Written")
        public List<Exam> Exams { get; set; }
    }
}
