using Pera.Entity.Entities;
using System.Collections.Generic;

namespace Pera.DataAccess.Abstract
{
    public interface IExamRepository
    {
        void AddExam(Exam exam);
        void AddExamResult(ExamResult result);
        List<ExamResult> GetResultsByStudent(string studentId);
        ExamResult GetDetail(int id);
        void Delete(int id);
        List<Exam> GetAllExams();
        List<ExamResult> GetAllResults();
        List<Course> GetCourses();
        void Update(ExamResult entity);
        void Add(ExamResult entity);
        ExamResult GetResult(int examId, string studentId);
        List<ExamResult> GetResultsByExamId(int examId);
    }
}
