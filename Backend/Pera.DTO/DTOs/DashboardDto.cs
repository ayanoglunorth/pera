using System;
using System.Collections.Generic;

namespace Pera.DTO.DTOs
{
    public class DashboardDto
    {
        // Student specific
        public int TotalExams { get; set; }
        public decimal AverageScore { get; set; }
        public int TotalGoals { get; set; }
        public int CompletedGoals { get; set; }
        public List<ExamListDto> UpcomingExams { get; set; }
        public List<ExamListDto> RecentResults { get; set; }
        public List<GoalDto> Goals { get; set; }

        // Teacher specific
        public int StudentCount { get; set; }
        public decimal ClassAverage { get; set; }
        public int PendingResults { get; set; }
    }
}