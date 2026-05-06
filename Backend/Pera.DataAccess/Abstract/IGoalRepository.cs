using Pera.Entity.Entities;
using System.Collections.Generic;

namespace Pera.DataAccess.Abstract
{
    public interface IGoalRepository
    {
        List<Goal> GetByUser(string userId);
        Goal GetById(int id);
        void Add(Goal goal);
        void Update(Goal goal);
        void Delete(int id);
    }
}