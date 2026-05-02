using System;
using System.Collections.Generic;

namespace Pera.DTO.DTOs
{
    // 1. COMMON PIECE (Used for both Input and Output)
    public class CourseResultDto
    {
        public string CourseName { get; set; }
        public int CorrectCount { get; set; }
        public int WrongCount { get; set; }
    }

    // 2. FOR DATA INPUT (Frontend -> Backend)
    // Used when teacher clicks "Save"
    public class ExamResultInputDto
    {
        public int ExamId { get; set; }
        public string StudentId { get; set; }
        public List<CourseResultDto> Courses { get; set; }
    }

    // 3. FOR DATA DISPLAY (Backend -> Frontend)
    // Used when student asks "What is my result?"
    public class ExamDetailDto
    {
        public int ExamId { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public decimal? Score { get; set; }
        public decimal TotalNet { get; set; }

        public List<CourseResultDto> Courses { get; set; }
    }
}
