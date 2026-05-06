using System;

namespace Pera.DTO.DTOs
{
    public class GoalDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public int CurrentValue { get; set; }
        public int TargetValue { get; set; }
        public DateTime? DueDate { get; set; }
        public bool IsCompleted { get; set; }
        public int ProgressPercent { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
    }

    public class CreateGoalDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public int TargetValue { get; set; }
        public DateTime? DueDate { get; set; }
        public string TeacherId { get; set; }
    }

    public class UpdateGoalDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int CurrentValue { get; set; }
        public int TargetValue { get; set; }
        public DateTime? DueDate { get; set; }
        public bool IsCompleted { get; set; }
    }
}