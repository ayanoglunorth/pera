using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pera.Entity.Entities
{
    [Table("Goals")]
    public class Goal
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string StudentId { get; set; }

        public string? TeacherId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }

        [Required]
        [MaxLength(50)]
        public string Type { get; set; }

        public int CurrentValue { get; set; }

        public int TargetValue { get; set; }

        public DateTime? DueDate { get; set; }

        public bool IsCompleted { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? CompletedAt { get; set; }

        [ForeignKey("StudentId")]
        public AppUser Student { get; set; }

        [ForeignKey("TeacherId")]
        public AppUser? Teacher { get; set; }
    }
}