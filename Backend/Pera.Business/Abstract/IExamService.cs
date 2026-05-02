using Pera.DTO.DTOs;
using Pera.Entity.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pera.Business.Abstract
{
    public interface IExamService
    {
        // 1. Exam Definition (Admin/Teacher)
        void DefineExam(ExamDefinitionDto model);

        // 2. Result Entry
        void EnterExamResult(ExamResultEntryDto model);

        // 3. List for Dropdown (Selection)
        List<ExamListDto> GetExamDefinitions(string type = null);

        // 4. List Student's Results
        List<ExamListDto> GetResultsByStudent(string studentId);

        // 5. Detail and Delete
        ExamDetailDto GetExamDetail(int resultId, string userId, string userRole);
        void DeleteExam(int id);

        List<ExamListDto> GetClassAverage();

        List<Course> GetAllCourses();

        // 1. Get Student List (Async because we use UserManager)
        Task<List<ExamStudentListDto>> GetExamStudentListAsync(int examId);

        // 2. Bulk Save
        void SaveBulkResults(BulkResultInputDto model);

        void AddExamResult(ExamResultInputDto model);
    }
}
