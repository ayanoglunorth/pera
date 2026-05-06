using Pera.DTO.DTOs;
using System.Collections.Generic;

namespace Pera.Business.Abstract
{
    public interface IGoalService
    {
        List<GoalDto> GetUserGoals(string userId);
        GoalDto GetGoalById(int id, string userId);
        void CreateGoal(CreateGoalDto model, string userId);
        void UpdateGoal(UpdateGoalDto model, string userId);
        void DeleteGoal(int id, string userId);
        void CompleteGoal(int id, string userId);
    }
}