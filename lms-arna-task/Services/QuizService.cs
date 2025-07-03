using lms_arna_task.Models;
using lms_arna_task.DTOs;
using lms_arna_task.Services.Interfaces;
using lms_arna_task.Repositories.Interfaces;

namespace lms_arna_task.Services
{
    public class QuizService : IQuizService
    {
        private readonly IQuizRepository _quizRepository;

        public QuizService(IQuizRepository quizRepository)
        {
            _quizRepository = quizRepository;
        }

        public async Task<AssignmentProgress?> GetUserProgressAsync(int userId, int assignmentId)
        {
            return await _quizRepository.GetUserProgressAsync(userId, assignmentId);
        }

        public async Task<List<AssignmentProgress>> GetAllUserProgressAsync(int userId)
        {
            return await _quizRepository.GetAllUserProgressAsync(userId);
        }

        public async Task<AssignmentProgress> StartAssignmentAsync(int userId, int assignmentId)
        {
            var existingProgress = await _quizRepository.GetUserProgressAsync(userId, assignmentId);
            if (existingProgress != null)
            {
                return existingProgress;
            }

            return await _quizRepository.StartAssignmentAsync(userId, assignmentId);
        }

        public async Task<AssignmentProgress> SubmitAnswersAsync(int userId, int assignmentId, Dictionary<int, string> answers)
        {
            return await _quizRepository.SubmitAnswersAsync(userId, assignmentId, answers);
        }

        public async Task<List<AssignmentProgress>> GetProgressReportAsync(int managerId)
        {
            return await _quizRepository.GetProgressReportAsync(managerId);
        }

        public async Task<bool> HasUserCompletedAssignmentAsync(int userId, int assignmentId)
        {
            return await _quizRepository.HasUserCompletedAssignmentAsync(userId, assignmentId);
        }

        public async Task<int> CalculateScoreAsync(int assignmentId, Dictionary<int, string> answers)
        {
            return await _quizRepository.CalculateScoreAsync(assignmentId, answers);
        }

        public async Task<UserAnswerDetails?> GetUserAnswerDetailsAsync(int userId, int assignmentId)
        {
            return await _quizRepository.GetUserAnswerDetailsAsync(userId, assignmentId);
        }
    }
}
