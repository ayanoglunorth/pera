using Microsoft.EntityFrameworkCore;
using Pera.DataAccess.Abstract;
using Pera.Entity.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Pera.DataAccess.Concrete
{
    public class ExamRepository : IExamRepository
    {
        private readonly AppDbContext _context;

        public ExamRepository(AppDbContext context)
        {
            _context = context;
        }

        // 1. Method to Add Exam Itself (THIS WAS MISSING)
        public void AddExam(Exam exam)
        {
            _context.Exams.Add(exam);
            _context.SaveChanges();
        }

        // 2. Method to Add Student Result
        public void AddExamResult(ExamResult examResult)
        {
            _context.ExamResults.Add(examResult);
            _context.SaveChanges();
        }

        // 3. Method to List Student's Exams
        public List<ExamResult> GetResultsByStudent(string studentId)
        {
            return _context.ExamResults
                .Include(x => x.Exam)
                .Where(x => x.StudentId == studentId)
                .OrderByDescending(x => x.Exam.Date)
                .ToList();
        }

        // 4. Method to Get Detail
        public ExamResult GetDetail(int id)
        {
            return _context.ExamResults
                .Include(x => x.Exam)
                .FirstOrDefault(x => x.Id == id);
        }

        public void Delete(int id)
        {
            var record = _context.ExamResults.Find(id);
            if (record != null)
            {
                _context.ExamResults.Remove(record);
                _context.SaveChanges();
            }
        }

        public List<Course> GetCourses()
        {
            return _context.Courses.ToList();
        }

        public List<Exam> GetAllExams()
        {
            return _context.Exams.OrderByDescending(x => x.Date).ToList();
        }

        public List<ExamResult> GetAllResults()
        {
            return _context.ExamResults
                .Include(x => x.Exam) // Required to get exam name
                .ToList();
        }

        // 1. Get all results for an exam
        public List<ExamResult> GetResultsByExamId(int examId)
        {
            return _context.ExamResults.Where(x => x.ExamId == examId).ToList();
        }

        // 2. Find a single student's result
        public ExamResult GetResult(int examId, string studentId)
        {
            return _context.ExamResults
                           .FirstOrDefault(x => x.ExamId == examId && x.StudentId == studentId);
        }

        // 3. Add and Update
        public void Add(ExamResult entity)
        {
            _context.ExamResults.Add(entity);
            _context.SaveChanges();
        }

        public void Update(ExamResult entity)
        {
            _context.ExamResults.Update(entity);
            _context.SaveChanges();
        }
    }
}
