using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Pera.Business.Abstract;
using Pera.DataAccess.Abstract;
using Pera.DTO.DTOs;
using Pera.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pera.Business.Concrete
{
    public class ExamService : IExamService
    {
        private readonly IExamRepository _examRepository;
        private readonly UserManager<AppUser> _userManager;

        public ExamService(IExamRepository examRepository, UserManager<AppUser> userManager)
        {
            _examRepository = examRepository;
            _userManager = userManager;
        }

        // 1. DEFINE EXAM
        public void DefineExam(ExamDefinitionDto model)
        {
            var exam = new Exam
            {
                Name = model.Name,
                Date = model.Date,
                Type = model.Type,
                CourseId = model.CourseId
            };
            _examRepository.AddExam(exam);
        }

        // 2. DETAILED EXAM RESULT ENTRY (WITH CORRECT/WRONG CALCULATION)
        public void EnterExamResult(ExamResultEntryDto model)
        {
            var result = new ExamResult
            {
                ExamId = model.ExamId,
                StudentId = model.StudentId,
                Score = 0
            };

            decimal totalNet = 0;

            foreach (var course in model.Courses)
            {
                decimal net = course.CorrectCount - (course.WrongCount / 3.0m);
                totalNet += net;

                switch (course.CourseName)
                {
                    case "Türkçe":
                        result.TurkishCorrect = course.CorrectCount; result.TurkishWrong = course.WrongCount; result.TurkishNet = net; break;
                    case "Matematik":
                        result.MathCorrect = course.CorrectCount; result.MathWrong = course.WrongCount; result.MathNet = net; break;
                    case "Fen Bilimleri":
                        result.ScienceCorrect = course.CorrectCount; result.ScienceWrong = course.WrongCount; result.ScienceNet = net; break;
                    case "İnkılap Tarihi":
                        result.HistoryCorrect = course.CorrectCount; result.HistoryWrong = course.WrongCount; result.HistoryNet = net; break;
                    case "Din Kültürü":
                        result.ReligionCorrect = course.CorrectCount; result.ReligionWrong = course.WrongCount; result.ReligionNet = net; break;
                    case "İngilizce":
                        result.EnglishCorrect = course.CorrectCount; result.EnglishWrong = course.WrongCount; result.EnglishNet = net; break;
                }
            }

            result.TotalNet = totalNet;
            result.Score = 194 + (totalNet * 4.0m);

            _examRepository.AddExamResult(result);
        }

        // 3. GET DEFINITIONS
        public List<ExamListDto> GetExamDefinitions(string type = null)
        {
            var exams = _examRepository.GetAllExams();

            if (!string.IsNullOrEmpty(type))
            {
                exams = exams.Where(x => x.Type == type).ToList();
            }

            return exams.Select(x => new ExamListDto
            {
                ExamId = x.Id,
                Name = x.Name,
                Date = x.Date
            }).ToList();
        }

        // 4. GET RESULTS (FOR STUDENT)
        public List<ExamListDto> GetResultsByStudent(string studentId)
        {
            var results = _examRepository.GetResultsByStudent(studentId);

            return results.Select(x => new ExamListDto
            {
                ExamId = x.Id,
                Name = x.Exam.Name,
                Date = x.Exam.Date,
                Score = x.Score,
                TotalNet = x.TotalNet,
                TurkishNet = x.TurkishNet,
                MathNet = x.MathNet,
                ScienceNet = x.ScienceNet,
                HistoryNet = x.HistoryNet,
                ReligionNet = x.ReligionNet,
                EnglishNet = x.EnglishNet
            }).ToList();
        }

        // 5. SHOW DETAIL
        public ExamDetailDto GetExamDetail(int resultId, string userId, string userRole)
        {
            var result = _examRepository.GetDetail(resultId);

            if (result == null) return null;

            // Security: If student, can only see own exam
            if (result.StudentId != userId && userRole != "Teacher")
            {
                return null;
            }

            return new ExamDetailDto
            {
                ExamId = result.Id,
                Name = result.Exam.Name,
                Date = result.Exam.Date,
                Score = result.Score,
                TotalNet = result.TotalNet,
                Courses = new List<CourseResultDto>
                {
                    new CourseResultDto { CourseName="Türkçe", CorrectCount=result.TurkishCorrect, WrongCount=result.TurkishWrong },
                    new CourseResultDto { CourseName="Matematik", CorrectCount=result.MathCorrect, WrongCount=result.MathWrong },
                    new CourseResultDto { CourseName="Fen Bilimleri", CorrectCount=result.ScienceCorrect, WrongCount=result.ScienceWrong },
                    new CourseResultDto { CourseName="İnkılap Tarihi", CorrectCount=result.HistoryCorrect, WrongCount=result.HistoryWrong },
                    new CourseResultDto { CourseName="Din Kültürü", CorrectCount=result.ReligionCorrect, WrongCount=result.ReligionWrong },
                    new CourseResultDto { CourseName="İngilizce", CorrectCount=result.EnglishCorrect, WrongCount=result.EnglishWrong }
                }
            };
        }

        // 6. DELETE
        public void DeleteExam(int id)
        {
            _examRepository.Delete(id);
        }

        // 7. CLASS AVERAGE
        public List<ExamListDto> GetClassAverage()
        {
            var allResults = _examRepository.GetAllResults();
            var grouped = allResults.GroupBy(x => x.ExamId);
            var list = new List<ExamListDto>();

            foreach (var group in grouped)
            {
                var examInfo = group.First().Exam;

                list.Add(new ExamListDto
                {
                    ExamId = examInfo.Id,
                    Name = examInfo.Name + " (Class Avg.)",
                    Date = examInfo.Date,
                    Score = Math.Round(group.Average(x => x.Score ?? 0), 2),
                    TotalNet = Math.Round(group.Average(x => x.TotalNet), 2),
                    TurkishNet = Math.Round(group.Average(x => x.TurkishNet), 2),
                    MathNet = Math.Round(group.Average(x => x.MathNet), 2),
                    ScienceNet = Math.Round(group.Average(x => x.ScienceNet), 2),
                    HistoryNet = Math.Round(group.Average(x => x.HistoryNet), 2),
                    ReligionNet = Math.Round(group.Average(x => x.ReligionNet), 2),
                    EnglishNet = Math.Round(group.Average(x => x.EnglishNet), 2)
                });
            }

            return list.OrderByDescending(x => x.Date).ToList();
        }

        // 8. GET COURSES
        public List<Course> GetAllCourses()
        {
            return _examRepository.GetCourses();
        }

        // 9. WRITTEN GRADE ENTRY - GET STUDENT LIST
        public async Task<List<ExamStudentListDto>> GetExamStudentListAsync(int examId)
        {
            var students = await _userManager.GetUsersInRoleAsync("Student");
            var existingResults = _examRepository.GetResultsByExamId(examId);

            var list = students.Select(std => new ExamStudentListDto
            {
                StudentId = std.Id,
                FullName = std.UserName,

                // FIX HERE: Added (int?) cast
                Score = (int?)(existingResults.FirstOrDefault(s => s.StudentId == std.Id)?.Score)
            }).ToList();

            return list;
        }

        // 10. WRITTEN GRADE ENTRY - BULK SAVE
        public void SaveBulkResults(BulkResultInputDto model)
        {
            foreach (var record in model.Records)
            {
                var existingResult = _examRepository.GetResult(model.ExamId, record.StudentId);

                if (existingResult != null)
                {
                    existingResult.Score = record.Score;
                    _examRepository.Update(existingResult);
                }
                else
                {
                    // When adding new record, fill other fields with 0 to avoid errors
                    var newResult = new ExamResult
                    {
                        ExamId = model.ExamId,
                        StudentId = record.StudentId,
                        Score = record.Score,

                        TotalNet = 0,
                        TurkishCorrect = 0,
                        TurkishWrong = 0,
                        TurkishNet = 0,
                        MathCorrect = 0,
                        MathWrong = 0,
                        MathNet = 0,
                        ScienceCorrect = 0,
                        ScienceWrong = 0,
                        ScienceNet = 0,
                        HistoryCorrect = 0,
                        HistoryWrong = 0,
                        HistoryNet = 0,
                        ReligionCorrect = 0,
                        ReligionWrong = 0,
                        ReligionNet = 0,
                        EnglishCorrect = 0,
                        EnglishWrong = 0,
                        EnglishNet = 0
                    };
                    _examRepository.Add(newResult);
                }
            }
        }

        // THIS METHOD REPLACES THE FAULTY ONE
        public void AddExamResult(ExamResultInputDto model)
        {
            // 1. First check if this student has a record for this exam
            var result = _examRepository.GetResult(model.ExamId, model.StudentId);

            bool isNewRecord = false;

            // If no record, create new
            if (result == null)
            {
                isNewRecord = true;
                result = new ExamResult
                {
                    ExamId = model.ExamId,
                    StudentId = model.StudentId,
                    Score = 0
                };
            }

            decimal totalNet = 0;

            // 2. Map incoming courses to your table columns one by one
            foreach (var course in model.Courses)
            {
                // LGS Logic: 3 Wrong = 1 Correct
                decimal net = course.CorrectCount - (course.WrongCount / 3.0m);
                totalNet += net;

                // MAPPING ACCORDING TO YOUR TABLE STRUCTURE
                // Frontend name ("Turkish" or "Türkçe") must match backend.
                // Here we provide flexibility with contains or tolower.
                var courseName = course.CourseName.ToLower();

                if (courseName.Contains("türkçe") || courseName.Contains("turkish"))
                {
                    result.TurkishCorrect = course.CorrectCount;
                    result.TurkishWrong = course.WrongCount;
                    result.TurkishNet = net;
                }
                else if (courseName.Contains("matematik") || courseName.Contains("math"))
                {
                    result.MathCorrect = course.CorrectCount;
                    result.MathWrong = course.WrongCount;
                    result.MathNet = net;
                }
                else if (courseName.Contains("fen") || courseName.Contains("science"))
                {
                    result.ScienceCorrect = course.CorrectCount;
                    result.ScienceWrong = course.WrongCount;
                    result.ScienceNet = net;
                }
                else if (courseName.Contains("inkılap") || courseName.Contains("inkilap") || courseName.Contains("history"))
                {
                    result.HistoryCorrect = course.CorrectCount;
                    result.HistoryWrong = course.WrongCount;
                    result.HistoryNet = net;
                }
                else if (courseName.Contains("din") || courseName.Contains("religion"))
                {
                    result.ReligionCorrect = course.CorrectCount;
                    result.ReligionWrong = course.WrongCount;
                    result.ReligionNet = net;
                }
                else if (courseName.Contains("ingilizce") || courseName.Contains("ing") || courseName.Contains("english"))
                {
                    result.EnglishCorrect = course.CorrectCount;
                    result.EnglishWrong = course.WrongCount;
                    result.EnglishNet = net;
                }
            }

            // 3. Calculate General Totals
            result.TotalNet = totalNet;
            // Simple Score Calculation (Adjust formula as needed)
            result.Score = 194 + (totalNet * 4.0m);

            // 4. Save to Database
            if (isNewRecord)
            {
                _examRepository.AddExamResult(result);
            }
            else
            {
                _examRepository.Update(result);
            }
        }
    }
}
