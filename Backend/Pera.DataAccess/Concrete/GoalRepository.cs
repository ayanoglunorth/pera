using Microsoft.EntityFrameworkCore;
using Pera.DataAccess.Abstract;
using Pera.Entity.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Pera.DataAccess.Concrete
{
    public class GoalRepository : IGoalRepository
    {
        private readonly AppDbContext _context;

        public GoalRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<Goal> GetByUser(string userId)
        {
            return _context.Goals
                .Where(g => g.StudentId == userId || g.TeacherId == userId)
                .OrderByDescending(g => g.CreatedAt)
                .ToList();
        }

        public Goal GetById(int id)
        {
            return _context.Goals.Find(id);
        }

        public void Add(Goal goal)
        {
            _context.Goals.Add(goal);
            _context.SaveChanges();
        }

        public void Update(Goal goal)
        {
            _context.Goals.Update(goal);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var goal = _context.Goals.Find(id);
            if (goal != null)
            {
                _context.Goals.Remove(goal);
                _context.SaveChanges();
            }
        }
    }
}