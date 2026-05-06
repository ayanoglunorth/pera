using Pera.Entity.Entities;
using System.Collections.Generic;

namespace Pera.DataAccess.Abstract
{
    public interface IUploadedResultRepository
    {
        List<UploadedResult> GetAll();
        List<UploadedResult> GetByTeacher(string teacherId);
        UploadedResult GetById(int id);
        void Add(UploadedResult entity);
        void Delete(int id);
    }
}