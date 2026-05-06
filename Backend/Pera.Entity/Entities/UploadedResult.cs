using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pera.Entity.Entities
{
    [Table("UploadedResults")]
    public class UploadedResult
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string TeacherId { get; set; }

        [Required]
        [MaxLength(500)]
        public string FileName { get; set; }

        [Required]
        [MaxLength(500)]
        public string StoredFileName { get; set; }

        [MaxLength(50)]
        public string FileType { get; set; }

        public long FileSize { get; set; }

        public DateTime UploadedAt { get; set; }

        [ForeignKey("TeacherId")]
        public AppUser Teacher { get; set; }
    }
}