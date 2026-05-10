using Microsoft.EntityFrameworkCore;
using Pera.DataAccess.Abstract;
using Pera.Entity.Entities;
using System.Collections.Generic;

namespace Pera.DataAccess.Concrete
{
    public class CourseRepository : ICourseRepository
    {
        private readonly AppDbContext _context;

        public CourseRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<Course> GetCoursesWithExams()
        {
            return _context.Courses
                .Include(d => d.Exams)
                .ThenInclude(s => s.Results)
                .ToList();
        }
    }
}
