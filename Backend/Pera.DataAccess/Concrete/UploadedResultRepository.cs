using Pera.DataAccess.Abstract;
using Pera.Entity.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Pera.DataAccess.Concrete
{
    public class UploadedResultRepository : IUploadedResultRepository
    {
        private readonly AppDbContext _context;

        public UploadedResultRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<UploadedResult> GetAll()
        {
            return _context.UploadedResults
                .OrderByDescending(r => r.UploadedAt)
                .ToList();
        }

        public List<UploadedResult> GetByTeacher(string teacherId)
        {
            return _context.UploadedResults
                .Where(r => r.TeacherId == teacherId)
                .OrderByDescending(r => r.UploadedAt)
                .ToList();
        }

        public UploadedResult GetById(int id)
        {
            return _context.UploadedResults.Find(id);
        }

        public void Add(UploadedResult entity)
        {
            _context.UploadedResults.Add(entity);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var result = _context.UploadedResults.Find(id);
            if (result != null)
            {
                _context.UploadedResults.Remove(result);
                _context.SaveChanges();
            }
        }
    }
}