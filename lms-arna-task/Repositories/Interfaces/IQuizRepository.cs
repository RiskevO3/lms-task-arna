using lms_arna_task.Models;

namespace lms_arna_task.Repositories.Interfaces
{
    public interface IQuizRepository
    {
        Task<IEnumerable<ProgressReportDto>> GetProgressReportAsync();
        Task<UserAnswerDetailsDto?> GetUserAnswerDetailsAsync(int userId, int assignmentId);
        Task<AssignmentProgress?> GetUserProgressAsync(int userId, int assignmentId);
        Task<List<AssignmentProgress>> GetAllUserProgressAsync(int userId);
        Task<AssignmentProgress> StartAssignmentAsync(int userId, int assignmentId);
        Task<AssignmentProgress> SubmitAnswersAsync(int userId, int assignmentId, Dictionary<int, string> answers);
        Task<List<AssignmentProgress>> GetProgressReportAsync(int managerId);
        Task<bool> HasUserCompletedAssignmentAsync(int userId, int assignmentId);
        Task<int> CalculateScoreAsync(int assignmentId, Dictionary<int, string> answers);
        Task<Question?> GetQuestionByIdAsync(int questionId);
        Task<List<Question>> GetQuestionsByAssignmentIdAsync(int assignmentId);
    }
}
