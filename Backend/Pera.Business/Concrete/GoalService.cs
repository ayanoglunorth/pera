using Pera.Business.Abstract;
using Pera.DataAccess.Abstract;
using Pera.DTO.DTOs;
using Pera.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pera.Business.Concrete
{
    public class GoalService : IGoalService
    {
        private readonly IGoalRepository _goalRepository;

        public GoalService(IGoalRepository goalRepository)
        {
            _goalRepository = goalRepository;
        }

        public List<GoalDto> GetUserGoals(string userId)
        {
            var goals = _goalRepository.GetByUser(userId);
            return goals.Select(MapToDto).ToList();
        }

        public GoalDto GetGoalById(int id, string userId)
        {
            var goal = _goalRepository.GetById(id);
            if (goal == null || (goal.StudentId != userId && goal.TeacherId != userId))
                return null;
            return MapToDto(goal);
        }

        public void CreateGoal(CreateGoalDto model, string userId)
        {
            var goal = new Goal
            {
                StudentId = userId,
                TeacherId = model.TeacherId,
                Title = model.Title,
                Description = model.Description,
                Type = model.Type,
                TargetValue = model.TargetValue,
                CurrentValue = 0,
                DueDate = model.DueDate,
                IsCompleted = false,
                CreatedAt = DateTime.Now
            };
            _goalRepository.Add(goal);
        }

        public void UpdateGoal(UpdateGoalDto model, string userId)
        {
            var goal = _goalRepository.GetById(model.Id);
            if (goal == null || (goal.StudentId != userId && goal.TeacherId != userId))
                return;

            goal.Title = model.Title;
            goal.Description = model.Description;
            goal.CurrentValue = model.CurrentValue;
            goal.TargetValue = model.TargetValue;
            goal.DueDate = model.DueDate;
            goal.IsCompleted = model.IsCompleted;
            _goalRepository.Update(goal);
        }

        public void DeleteGoal(int id, string userId)
        {
            var goal = _goalRepository.GetById(id);
            if (goal == null || (goal.StudentId != userId && goal.TeacherId != userId))
                return;
            _goalRepository.Delete(id);
        }

        public void CompleteGoal(int id, string userId)
        {
            var goal = _goalRepository.GetById(id);
            if (goal == null || (goal.StudentId != userId && goal.TeacherId != userId))
                return;
            goal.IsCompleted = true;
            goal.CompletedAt = DateTime.Now;
            _goalRepository.Update(goal);
        }

        private GoalDto MapToDto(Goal goal)
        {
            return new GoalDto
            {
                Id = goal.Id,
                Title = goal.Title,
                Description = goal.Description,
                Type = goal.Type,
                CurrentValue = goal.CurrentValue,
                TargetValue = goal.TargetValue,
                DueDate = goal.DueDate,
                IsCompleted = goal.IsCompleted,
                ProgressPercent = goal.TargetValue > 0 ? (goal.CurrentValue * 100) / goal.TargetValue : 0,
                CreatedAt = goal.CreatedAt,
                CompletedAt = goal.CompletedAt
            };
        }
    }
}