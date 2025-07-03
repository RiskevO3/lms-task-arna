namespace lms_arna_task.Models
{
    public class QuestionDetailDto
    {
        public string? QuestionText { get; set; }
        public string[]? Options { get; set; }
        public string? SelectedAnswer { get; set; }
        public string? CorrectAnswer { get; set; }
        public bool IsCorrect { get; set; }
    }
}
