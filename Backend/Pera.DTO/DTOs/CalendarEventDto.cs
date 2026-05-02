using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pera.DTO.DTOs
{
    public class CalendarEventDto
    {
        public int Id { get; set; }
        public string Title { get; set; }   // Example: "Math 1st Written Exam"
        public DateTime Date { get; set; }
        public string Description { get; set; } 
        public string Color { get; set; }     // Example: "#FF5733" (Orange) - Frontend will paint with this color
    }
}
