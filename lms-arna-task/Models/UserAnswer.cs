using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lms_arna_task.Models
{
    public class UserAnswer
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; } = null!;

        public int QuestionId { get; set; }

        [ForeignKey("QuestionId")]
        public Question Question { get; set; } = null!;

        public int AssignmentProgressId { get; set; }

        [ForeignKey("AssignmentProgressId")]
        public AssignmentProgress AssignmentProgress { get; set; } = null!;

        [Required]
        [MaxLength(1)]
        public string SelectedAnswer { get; set; } = string.Empty; // A, B, C, D

        public bool IsCorrect { get; set; } = false;

        public DateTime AnsweredAt { get; set; } = DateTime.UtcNow;
    }
}
