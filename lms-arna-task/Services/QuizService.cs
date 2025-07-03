using Microsoft.EntityFrameworkCore;
using lms_arna_task.Data;
using lms_arna_task.Models;
using lms_arna_task.Services.Interfaces;

namespace lms_arna_task.Services
{
    public class QuizService : IQuizService
    {
        private readonly ApplicationDbContext _context;

        public QuizService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<AssignmentProgress?> GetUserProgressAsync(int userId, int assignmentId)
        {
            return await _context.AssignmentProgresses
                .Include(ap => ap.Assignment)
                .Include(ap => ap.User)
                .Include(ap => ap.UserAnswers)
                .ThenInclude(ua => ua.Question)
                .FirstOrDefaultAsync(ap => ap.UserId == userId && ap.AssignmentId == assignmentId);
        }

        public async Task<List<AssignmentProgress>> GetAllUserProgressAsync(int userId)
        {
            return await _context.AssignmentProgresses
                .Include(ap => ap.Assignment)
                .Include(ap => ap.User)
                .Include(ap => ap.UserAnswers)
                .ThenInclude(ua => ua.Question)
                .Where(ap => ap.UserId == userId)
                .OrderByDescending(ap => ap.UpdatedAt)
                .ToListAsync();
        }

        public async Task<AssignmentProgress> StartAssignmentAsync(int userId, int assignmentId)
        {
            var existingProgress = await GetUserProgressAsync(userId, assignmentId);
            if (existingProgress != null)
            {
                return existingProgress;
            }

            var progress = new AssignmentProgress
            {
                UserId = userId,
                AssignmentId = assignmentId,
                StartedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.AssignmentProgresses.Add(progress);
            await _context.SaveChangesAsync();
            return progress;
        }

        public async Task<AssignmentProgress> SubmitAnswersAsync(int userId, int assignmentId, Dictionary<int, string> answers)
        {
            var progress = await GetUserProgressAsync(userId, assignmentId);
            if (progress == null)
            {
                progress = await StartAssignmentAsync(userId, assignmentId);
            }

            if (progress.IsCompleted)
            {
                throw new InvalidOperationException("Assignment has already been completed");
            }

            // Calculate score
            var score = await CalculateScoreAsync(assignmentId, answers);

            // Save user answers
            var userAnswers = new List<UserAnswer>();
            foreach (var answer in answers)
            {
                var question = await _context.Questions.FindAsync(answer.Key);
                if (question != null)
                {
                    var userAnswer = new UserAnswer
                    {
                        UserId = userId,
                        QuestionId = answer.Key,
                        AssignmentProgressId = progress.Id,
                        SelectedAnswer = answer.Value,
                        IsCorrect = question.CorrectAnswer == answer.Value,
                        AnsweredAt = DateTime.UtcNow
                    };
                    userAnswers.Add(userAnswer);
                }
            }

            _context.UserAnswers.AddRange(userAnswers);

            // Update progress
            progress.Score = score;
            progress.IsCompleted = true;
            progress.CompletedAt = DateTime.UtcNow;
            progress.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return progress;
        }

        public async Task<List<AssignmentProgress>> GetProgressReportAsync(int managerId)
        {
            // Get all active assignments
            var activeAssignments = await _context.Assignments
                .Where(a => a.IsActive && a.CreatedById == managerId)
                .ToListAsync();

            if (!activeAssignments.Any())
            {
                return new List<AssignmentProgress>();
            }

            // Get all learners managed by this manager
            var managedLearners = await _context.Users
                .Where(u => u.ManagerId == managerId && u.Role == "Learner")
                .ToListAsync();

            var result = new List<AssignmentProgress>();

            // For each assignment and each learner, create or get their progress
            foreach (var assignment in activeAssignments)
            {
                foreach (var learner in managedLearners)
                {
                    // Try to find existing progress
                    var existingProgress = await _context.AssignmentProgresses
                        .Include(ap => ap.User)
                        .Include(ap => ap.Assignment)
                        .FirstOrDefaultAsync(ap => ap.UserId == learner.Id && ap.AssignmentId == assignment.Id);

                    if (existingProgress != null)
                    {
                        // Add existing progress
                        result.Add(existingProgress);
                    }
                    else
                    {
                        // Create a virtual progress entry for learners who haven't started
                        var virtualProgress = new AssignmentProgress
                        {
                            Id = 0, // Virtual entry
                            UserId = learner.Id,
                            User = learner,
                            AssignmentId = assignment.Id,
                            Assignment = assignment,
                            Score = 0,
                            IsCompleted = false,
                            CompletedAt = null,
                            StartedAt = DateTime.MinValue, // Indicates not started
                            UpdatedAt = DateTime.MinValue,
                            UserAnswers = new List<UserAnswer>()
                        };
                        result.Add(virtualProgress);
                    }
                }
            }

            return result.OrderByDescending(ap => ap.UpdatedAt).ToList();
        }

        public async Task<bool> HasUserCompletedAssignmentAsync(int userId, int assignmentId)
        {
            return await _context.AssignmentProgresses
                .AnyAsync(ap => ap.UserId == userId && ap.AssignmentId == assignmentId && ap.IsCompleted);
        }

        public async Task<int> CalculateScoreAsync(int assignmentId, Dictionary<int, string> answers)
        {
            var questions = await _context.Questions
                .Where(q => q.AssignmentId == assignmentId)
                .ToListAsync();

            var totalQuestions = questions.Count;
            var correctAnswers = 0;

            foreach (var question in questions)
            {
                if (answers.TryGetValue(question.Id, out var selectedAnswer) &&
                    selectedAnswer == question.CorrectAnswer)
                {
                    correctAnswers++;
                }
            }

            // Calculate score: 1 correct answer = 20 points (for 5 questions = 100 points total)
            return totalQuestions > 0 ? (correctAnswers * 100) / totalQuestions : 0;
        }

        public async Task<UserAnswerDetailsDto?> GetUserAnswerDetailsAsync(int userId, int assignmentId)
        {
            var progress = await _context.AssignmentProgresses
                .Include(ap => ap.Assignment)
                .Include(ap => ap.User)
                .Include(ap => ap.UserAnswers)
                .ThenInclude(ua => ua.Question)
                .FirstOrDefaultAsync(ap => ap.UserId == userId && ap.AssignmentId == assignmentId && ap.IsCompleted);

            if (progress == null)
            {
                return null;
            }

            var answerDetails = new UserAnswerDetailsDto
            {
                UserName = progress.User?.Username ?? "Unknown",
                UserEmail = progress.User?.Email ?? "Unknown",
                AssignmentTitle = progress.Assignment?.Title ?? "Unknown",
                AssignmentDescription = progress.Assignment?.Description ?? "Unknown",
                Score = progress.Score,
                CompletedAt = progress.CompletedAt,
                TotalQuestions = progress.UserAnswers.Count,
                CorrectAnswers = progress.UserAnswers.Count(ua => ua.IsCorrect),
                Answers = progress.UserAnswers.Select(ua => new QuestionDetailDto
                {
                    QuestionText = ua.Question?.QuestionText ?? "Unknown",
                    SelectedAnswer = ua.SelectedAnswer,
                    CorrectAnswer = ua.Question?.CorrectAnswer ?? "Unknown",
                    IsCorrect = ua.IsCorrect,
                    Options = new[]
                    {
                        ua.Question?.OptionA ?? "Unknown",
                        ua.Question?.OptionB ?? "Unknown",
                        ua.Question?.OptionC ?? "Unknown",
                        ua.Question?.OptionD ?? "Unknown"
                    }
                }).ToList()
            };

            return answerDetails;
        }
    }
}
