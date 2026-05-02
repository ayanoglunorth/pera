using Pera.DTO.DTOs;
using System.Collections.Generic;

namespace Pera.Business.Abstract
{
    public interface ICourseService
    {
        // UPDATE: Added (string studentId) parameter to method.
        // So when requesting grades from service, we can ask "Which student's grades?"
        List<CourseGradesDto> GetStudentCourseGrades(string studentId);
    }
}
