using System;

namespace Pera.DTO.DTOs
{
    public class SettingsDto
    {
        public string UserId { get; set; }
        public bool EmailNotifications { get; set; }
        public bool ExamReminders { get; set; }
        public bool ResultNotifications { get; set; }
        public bool MessageNotifications { get; set; }
        public string Theme { get; set; }
        public string Language { get; set; }
    }

    public class UpdateSettingsDto
    {
        public bool EmailNotifications { get; set; }
        public bool ExamReminders { get; set; }
        public bool ResultNotifications { get; set; }
        public bool MessageNotifications { get; set; }
        public string Theme { get; set; }
        public string Language { get; set; }
    }
}