namespace lms_arna_task.DTOs
{
    public class QuestionDetail
    {
        public string? QuestionText { get; set; }
        public string[]? Options { get; set; }
        public string? SelectedAnswer { get; set; }
        public string? CorrectAnswer { get; set; }
        public bool IsCorrect { get; set; }
    }
}
