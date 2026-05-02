using System;
using System.Collections.Generic;

namespace Pera.DTO.DTOs
{
    // 1. DTO USED WHEN ENTERING RESULT (Teacher entering grades)
    public class ExamResultEntryDto
    {
        // NO NAME ANYMORE! Only ID.
        // Because the exam name (e.g., "Töder-1") is already stored in database.
        public int ExamId { get; set; }

        public string StudentId { get; set; } // Student's ID (as GUID string)

        public List<CourseResultEntryDto> Courses { get; set; }
    }

    // Each row of the table (Your class called DersSonucSatiri)
    // We used DersEkleDto name in Service code, so changed accordingly.
    public class CourseResultEntryDto
    {
        public string CourseName { get; set; }
        public int CorrectCount { get; set; }
        public int WrongCount { get; set; }
    }

    // 2. DTO USED WHEN DEFINING NEW EXAM (Only Admin/Teacher)
    // We will use this in the "Define Exam" page.
    public class ExamDefinitionDto
    {
        public string Name { get; set; }    // Example: "Töder Turkey General - 1"
        public DateTime Date { get; set; }

        public string Type { get; set; }

        public int? CourseId { get; set; }
    }
}
