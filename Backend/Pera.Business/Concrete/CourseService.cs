using Pera.Business.Abstract;
using Pera.DataAccess.Abstract;
using Pera.DTO.DTOs;
using System.Collections.Generic;
using System.Linq;

namespace Pera.Business.Concrete
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;

        public CourseService(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        // NOTE: Added 'studentId' parameter to method.
        // Because we need to know "Which student's grades?" when requesting from service.
        public List<CourseGradesDto> GetStudentCourseGrades(string studentId)
        {
            // 1. Fetch Courses and linked Exams from Database
            var incomingData = _courseRepository.GetCoursesWithExams();

            // 2. Entity -> DTO Conversion
            var resultList = incomingData.Select(course => new CourseGradesDto
            {
                CourseName = course.Name,

                // Iterate through 'Exams' list inside Course
                // ONLY filter 'Written' ones so Exam Exams (LGS etc.) don't mix here.
                Exams = course.Exams
                    .Where(x => x.Type == "Written")
                    .Select(exam => new ExamGradeDto
                    {
                        ExamName = exam.Name,
                        Date = exam.Date.ToString("dd.MM.yyyy"),

                        // CRITICAL POINT: From results of that exam,
                        // find ONLY our student's (studentId) result.
                        Score = exam.Results
                           .Where(r => r.StudentId == studentId)
                           .Select(r => (decimal?)r.Score)
                           .FirstOrDefault() ?? 0
                    }).ToList()

            }).ToList();

            return resultList;
        }
    }
}
