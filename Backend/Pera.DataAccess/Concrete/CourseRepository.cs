using Microsoft.EntityFrameworkCore;
using Pera.DataAccess.Abstract;
using Pera.Entity.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Pera.DataAccess.Concrete
{
    public class CourseRepository : ICourseRepository
    {
        public List<Course> GetCoursesWithExams()
        {
            // NOTE: Here we create a new context inside 'using' block.
            // This method reduces the chance of "Dependency Injection" errors to zero.
            using (var context = new AppDbContext())
            {
                return context.Courses
                    .Include(d => d.Exams)       // Get Course and its Exams
                    .ThenInclude(s => s.Results)    // Also get Exam results
                    .ToList();
            }
        }
    }
}
