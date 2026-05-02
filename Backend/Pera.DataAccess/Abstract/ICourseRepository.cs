using System.Collections.Generic;
using Pera.Entity.Entities;

namespace Pera.DataAccess.Abstract
{
    public interface ICourseRepository
    {
        // Keep only what we need to avoid confusion
        List<Course> GetCoursesWithExams();
    }
}
