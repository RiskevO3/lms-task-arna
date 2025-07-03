using Microsoft.EntityFrameworkCore;
using lms_arna_task.Data;
using lms_arna_task.Models;
using lms_arna_task.Repositories;
using lms_arna_task.Services;

namespace lms_arna_task.Tests
{
    public class QuizScoringTests
    {
        private ApplicationDbContext GetInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new ApplicationDbContext(options);
            SeedTestData(context);
            return context;
        }

        private void SeedTestData(ApplicationDbContext context)
        {
            // Create test assignment
            var assignment = new Assignment
            {
                Id = 1,
                Title = "Test Assignment",
                Description = "Test Description",
                IsActive = true,
                StartDate = DateTime.UtcNow.AddDays(-1),
                EndDate = DateTime.UtcNow.AddDays(1),
                CreatedById = 1,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            context.Assignments.Add(assignment);

            // Create test questions
            var questions = new List<Question>
            {
                new Question
                {
                    Id = 1,
                    QuestionText = "What is 2+2?",
                    OptionA = "3",
                    OptionB = "4",
                    OptionC = "5",
                    OptionD = "6",
                    CorrectAnswer = "B",
                    AssignmentId = 1
                },
                new Question
                {
                    Id = 2,
                    QuestionText = "What is 3+3?",
                    OptionA = "5",
                    OptionB = "6",
                    OptionC = "7",
                    OptionD = "8",
                    CorrectAnswer = "B",
                    AssignmentId = 1
                },
                new Question
                {
                    Id = 3,
                    QuestionText = "What is 4+4?",
                    OptionA = "7",
                    OptionB = "8",
                    OptionC = "9",
                    OptionD = "10",
                    CorrectAnswer = "B",
                    AssignmentId = 1
                },
                new Question
                {
                    Id = 4,
                    QuestionText = "What is 5+5?",
                    OptionA = "9",
                    OptionB = "10",
                    OptionC = "11",
                    OptionD = "12",
                    CorrectAnswer = "B",
                    AssignmentId = 1
                },
                new Question
                {
                    Id = 5,
                    QuestionText = "What is 6+6?",
                    OptionA = "11",
                    OptionB = "12",
                    OptionC = "13",
                    OptionD = "14",
                    CorrectAnswer = "B",
                    AssignmentId = 1
                }
            };

            context.Questions.AddRange(questions);
            context.SaveChanges();
        }

        [Fact]
        public async Task CalculateScore_AllCorrectAnswers_Returns100Percent()
        {
            // Arrange
            using var context = GetInMemoryContext();
            var repository = new QuizRepository(context);
            var service = new QuizService(repository);

            var answers = new Dictionary<int, string>
            {
                { 1, "B" }, // Correct
                { 2, "B" }, // Correct
                { 3, "B" }, // Correct
                { 4, "B" }, // Correct
                { 5, "B" }  // Correct
            };

            // Act
            var score = await service.CalculateScoreAsync(1, answers);

            // Assert
            Assert.Equal(100, score);
        }

        [Fact]
        public async Task CalculateScore_NoCorrectAnswers_Returns0Percent()
        {
            // Arrange
            using var context = GetInMemoryContext();
            var repository = new QuizRepository(context);
            var service = new QuizService(repository);

            var answers = new Dictionary<int, string>
            {
                { 1, "A" }, // Incorrect
                { 2, "A" }, // Incorrect
                { 3, "A" }, // Incorrect
                { 4, "A" }, // Incorrect
                { 5, "A" }  // Incorrect
            };

            // Act
            var score = await service.CalculateScoreAsync(1, answers);

            // Assert
            Assert.Equal(0, score);
        }

        [Fact]
        public async Task CalculateScore_HalfCorrectAnswers_Returns60Percent()
        {
            // Arrange
            using var context = GetInMemoryContext();
            var repository = new QuizRepository(context);
            var service = new QuizService(repository);

            var answers = new Dictionary<int, string>
            {
                { 1, "B" }, // Correct
                { 2, "B" }, // Correct
                { 3, "B" }, // Correct
                { 4, "A" }, // Incorrect
                { 5, "A" }  // Incorrect
            };

            // Act
            var score = await service.CalculateScoreAsync(1, answers);

            // Assert
            Assert.Equal(60, score); // 3 out of 5 correct = 60%
        }

        [Fact]
        public async Task CalculateScore_PartialAnswers_CalculatesBasedOnAnsweredQuestions()
        {
            // Arrange
            using var context = GetInMemoryContext();
            var repository = new QuizRepository(context);
            var service = new QuizService(repository);

            var answers = new Dictionary<int, string>
            {
                { 1, "B" }, // Correct
                { 2, "A" }  // Incorrect (only answering 2 out of 5 questions)
            };

            // Act
            var score = await service.CalculateScoreAsync(1, answers);

            // Assert
            // Should calculate based on all questions (5), not just answered ones
            // 1 correct out of 5 total = 20%
            Assert.Equal(20, score);
        }

        [Fact]
        public async Task CalculateScore_EmptyAnswers_Returns0Percent()
        {
            // Arrange
            using var context = GetInMemoryContext();
            var repository = new QuizRepository(context);
            var service = new QuizService(repository);

            var answers = new Dictionary<int, string>();

            // Act
            var score = await service.CalculateScoreAsync(1, answers);

            // Assert
            Assert.Equal(0, score);
        }

        [Fact]
        public async Task CalculateScore_NonExistentAssignment_Returns0Percent()
        {
            // Arrange
            using var context = GetInMemoryContext();
            var repository = new QuizRepository(context);
            var service = new QuizService(repository);

            var answers = new Dictionary<int, string>
            {
                { 1, "B" }
            };

            // Act
            var score = await service.CalculateScoreAsync(999, answers); // Non-existent assignment

            // Assert
            Assert.Equal(0, score);
        }

        [Fact]
        public async Task CalculateScore_OneCorrectOutOfFive_Returns20Percent()
        {
            // Arrange
            using var context = GetInMemoryContext();
            var repository = new QuizRepository(context);
            var service = new QuizService(repository);

            var answers = new Dictionary<int, string>
            {
                { 1, "B" }, // Correct
                { 2, "A" }, // Incorrect
                { 3, "A" }, // Incorrect
                { 4, "A" }, // Incorrect
                { 5, "A" }  // Incorrect
            };

            // Act
            var score = await service.CalculateScoreAsync(1, answers);

            // Assert
            Assert.Equal(20, score); // 1 out of 5 correct = 20%
        }

        [Fact]
        public async Task CalculateScore_FourCorrectOutOfFive_Returns80Percent()
        {
            // Arrange
            using var context = GetInMemoryContext();
            var repository = new QuizRepository(context);
            var service = new QuizService(repository);

            var answers = new Dictionary<int, string>
            {
                { 1, "B" }, // Correct
                { 2, "B" }, // Correct
                { 3, "B" }, // Correct
                { 4, "B" }, // Correct
                { 5, "A" }  // Incorrect
            };

            // Act
            var score = await service.CalculateScoreAsync(1, answers);

            // Assert
            Assert.Equal(80, score); // 4 out of 5 correct = 80%
        }
    }
}
