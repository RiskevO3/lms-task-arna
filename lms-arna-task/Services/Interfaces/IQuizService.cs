using lms_arna_task.Models;
using lms_arna_task.DTOs;

namespace lms_arna_task.Services.Interfaces
{
    public interface IQuizService
    {
        Task<AssignmentProgress?> GetUserProgressAsync(int userId, int assignmentId);
        Task<List<AssignmentProgress>> GetAllUserProgressAsync(int userId);
        Task<AssignmentProgress> StartAssignmentAsync(int userId, int assignmentId);
        Task<AssignmentProgress> SubmitAnswersAsync(int userId, int assignmentId, Dictionary<int, string> answers);
        Task<List<AssignmentProgress>> GetProgressReportAsync(int managerId);
        Task<bool> HasUserCompletedAssignmentAsync(int userId, int assignmentId);
        Task<int> CalculateScoreAsync(int assignmentId, Dictionary<int, string> answers);
        Task<UserAnswerDetails?> GetUserAnswerDetailsAsync(int userId, int assignmentId);
    }
}
