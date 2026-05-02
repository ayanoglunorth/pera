using System.Collections.Generic;

namespace Pera.DTO.DTOs
{
    // 1. List we will send to Frontend (Student + Score)
    public class ExamStudentListDto
    {
        public string StudentId { get; set; }
        public string FullName { get; set; }
        public int? Score { get; set; } // Can be null (if grade not entered)
    }

    // 2. Save package coming from Frontend
    public class BulkResultInputDto
    {
        public int ExamId { get; set; }
        public List<StudentGradeDto> Records { get; set; }
    }

    public class StudentGradeDto
    {
        public string StudentId { get; set; }
        public int Score { get; set; }
    }
}
