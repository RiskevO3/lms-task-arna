using lms_arna_task.Data;
using lms_arna_task.Models;
using lms_arna_task.DTOs;
using lms_arna_task.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace lms_arna_task.Repositories
{
    public class QuizRepository : IQuizRepository
    {
        private readonly ApplicationDbContext _context;

        public QuizRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProgressReport>> GetProgressReportAsync()
        {
            return await _context.AssignmentProgresses
                .Include(p => p.User)
                .Include(p => p.Assignment)
                .Select(p => new ProgressReport
                {
                    UserId = p.UserId,
                    Username = p.User.Username,
                    Email = p.User.Email,
                    AssignmentId = p.AssignmentId,
                    AssignmentTitle = p.Assignment.Title,
                    AssignmentDescription = p.Assignment.Description,
                    Score = p.Score,
                    StartedAt = p.StartedAt,
                    CompletedAt = p.CompletedAt,
                    IsCompleted = p.IsCompleted,
                    UpdatedAt = p.UpdatedAt
                })
                .ToListAsync();
        }

        public async Task<UserAnswerDetails?> GetUserAnswerDetailsAsync(int userId, int assignmentId)
        {
            var assignmentProgress = await _context.AssignmentProgresses
                .Include(ap => ap.User)
                .Include(ap => ap.Assignment)
                .FirstOrDefaultAsync(ap => ap.UserId == userId && ap.AssignmentId == assignmentId);

            if (assignmentProgress == null)
            {
                return null;
            }

            var userAnswers = await _context.UserAnswers
                .Where(ua => ua.UserId == userId && ua.AssignmentProgressId == assignmentProgress.Id)
                .Include(ua => ua.Question)
                .ToListAsync();

            var questionDetails = userAnswers.Select(ua => new QuestionDetail
            {
                QuestionText = ua.Question.QuestionText,
                Options = new[] { ua.Question.OptionA, ua.Question.OptionB, ua.Question.OptionC, ua.Question.OptionD },
                SelectedAnswer = ua.SelectedAnswer,
                CorrectAnswer = ua.Question.CorrectAnswer,
                IsCorrect = ua.IsCorrect
            }).ToList();

            return new UserAnswerDetails
            {
                UserName = assignmentProgress.User.Username,
                UserEmail = assignmentProgress.User.Email,
                AssignmentTitle = assignmentProgress.Assignment.Title,
                AssignmentDescription = assignmentProgress.Assignment.Description,
                Score = assignmentProgress.Score,
                CorrectAnswers = questionDetails.Count(q => q.IsCorrect),
                TotalQuestions = questionDetails.Count,
                Answers = questionDetails
            };
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

            return totalQuestions > 0 ? (correctAnswers * 100) / totalQuestions : 0;
        }

        public async Task<Question?> GetQuestionByIdAsync(int questionId)
        {
            return await _context.Questions.FindAsync(questionId);
        }

        public async Task<List<Question>> GetQuestionsByAssignmentIdAsync(int assignmentId)
        {
            return await _context.Questions
                .Where(q => q.AssignmentId == assignmentId)
                .ToListAsync();
        }
    }
}
