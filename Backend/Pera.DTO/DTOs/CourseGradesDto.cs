using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pera.DTO.DTOs
{
    public class CourseGradesDto
    {
        public int CourseId { get; set; }
        public string CourseName {  get; set; }
        public List<ExamGradeDto> Exams { get; set; }
    }
    public class ExamGradeDto
    {
        public string ExamName { get; set; }
        public string Date { get; set; }

        public decimal Score {  get; set; }
    }
}
