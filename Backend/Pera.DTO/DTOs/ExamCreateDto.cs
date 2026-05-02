using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pera.DTO.DTOs
{
    public class ExamCreateDto
    {
        public string Title { get; set; } // Example: "TYT Exam 1"
        public DateTime Date { get; set; }
        public int CourseId { get; set; }    // Which course's exam?
    }
}
