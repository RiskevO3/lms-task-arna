using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lms_arna_task.Models
{
    public class Assignment
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public string MaterialContent { get; set; } = string.Empty; // PDF link or video embed

        [Required]
        public string MaterialType { get; set; } = string.Empty; // "PDF", "Video", "Text"

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public bool IsActive { get; set; } = true;

        public int CreatedById { get; set; }

        [ForeignKey("CreatedById")]
        public User CreatedBy { get; set; } = null!;

        public ICollection<Question> Questions { get; set; } = new List<Question>();

        public ICollection<AssignmentProgress> AssignmentProgresses { get; set; } = new List<AssignmentProgress>();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
