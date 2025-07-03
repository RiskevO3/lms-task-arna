using Microsoft.EntityFrameworkCore;
using lms_arna_task.Models;

namespace lms_arna_task.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<AssignmentProgress> AssignmentProgresses { get; set; }
        public DbSet<UserAnswer> UserAnswers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User self-referencing relationship
            modelBuilder.Entity<User>()
                .HasOne(u => u.Manager)
                .WithMany(u => u.Subordinates)
                .HasForeignKey(u => u.ManagerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Assignment created by User
            modelBuilder.Entity<Assignment>()
                .HasOne(a => a.CreatedBy)
                .WithMany(u => u.CreatedAssignments)
                .HasForeignKey(a => a.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            // Question belongs to Assignment
            modelBuilder.Entity<Question>()
                .HasOne(q => q.Assignment)
                .WithMany(a => a.Questions)
                .HasForeignKey(q => q.AssignmentId)
                .OnDelete(DeleteBehavior.Cascade);

            // AssignmentProgress relationships
            modelBuilder.Entity<AssignmentProgress>()
                .HasOne(ap => ap.User)
                .WithMany(u => u.AssignmentProgresses)
                .HasForeignKey(ap => ap.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AssignmentProgress>()
                .HasOne(ap => ap.Assignment)
                .WithMany(a => a.AssignmentProgresses)
                .HasForeignKey(ap => ap.AssignmentId)
                .OnDelete(DeleteBehavior.Cascade);

            // UserAnswer relationships
            modelBuilder.Entity<UserAnswer>()
                .HasOne(ua => ua.User)
                .WithMany()
                .HasForeignKey(ua => ua.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserAnswer>()
                .HasOne(ua => ua.Question)
                .WithMany(q => q.UserAnswers)
                .HasForeignKey(ua => ua.QuestionId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserAnswer>()
                .HasOne(ua => ua.AssignmentProgress)
                .WithMany(ap => ap.UserAnswers)
                .HasForeignKey(ua => ua.AssignmentProgressId)
                .OnDelete(DeleteBehavior.Cascade);

            // Unique constraints
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<AssignmentProgress>()
                .HasIndex(ap => new { ap.UserId, ap.AssignmentId })
                .IsUnique();

            // Seed data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Create default manager and learner users
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Username = "manager",
                    Email = "manager@example.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                    Role = "Manager",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new User
                {
                    Id = 2,
                    Username = "learner1",
                    Email = "learner1@example.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                    Role = "Learner",
                    ManagerId = 1,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new User
                {
                    Id = 3,
                    Username = "learner2",
                    Email = "learner2@example.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                    Role = "Learner",
                    ManagerId = 1,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new User
                {
                    Id = 4,
                    Username = "learner3",
                    Email = "learner3@example.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                    Role = "Learner",
                    ManagerId = 1,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new User
                {
                    Id = 5,
                    Username = "learner4",
                    Email = "learner4@example.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                    Role = "Learner",
                    ManagerId = 1,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new User
                {
                    Id = 6,
                    Username = "learner5",
                    Email = "learner5@example.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                    Role = "Learner",
                    ManagerId = 1,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new User
                {
                    Id = 7,
                    Username = "learner6",
                    Email = "learner6@example.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                    Role = "Learner",
                    ManagerId = 1,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new User
                {
                    Id = 8,
                    Username = "learner7",
                    Email = "learner7@example.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                    Role = "Learner",
                    ManagerId = 1,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new User
                {
                    Id = 9,
                    Username = "learner8",
                    Email = "learner8@example.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                    Role = "Learner",
                    ManagerId = 1,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new User
                {
                    Id = 10,
                    Username = "learner9",
                    Email = "learner9@example.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                    Role = "Learner",
                    ManagerId = 1,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new User
                {
                    Id = 11,
                    Username = "learner10",
                    Email = "learner10@example.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                    Role = "Learner",
                    ManagerId = 1,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            );

            // Sample assignment
            modelBuilder.Entity<Assignment>().HasData(
                new Assignment
                {
                    Id = 1,
                    Title = "Introduction to Programming",
                    Description = "Learn the basics of programming concepts",
                    MaterialContent = "https://example.com/programming-basics.pdf",
                    MaterialType = "PDF",
                    StartDate = DateTime.UtcNow.AddDays(-7),
                    EndDate = DateTime.UtcNow.AddDays(7),
                    IsActive = true,
                    CreatedById = 1,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            );

            // Sample questions
            modelBuilder.Entity<Question>().HasData(
                new Question
                {
                    Id = 1,
                    QuestionText = "What is a variable in programming?",
                    OptionA = "A fixed value",
                    OptionB = "A container for storing data",
                    OptionC = "A function",
                    OptionD = "A loop",
                    CorrectAnswer = "B",
                    AssignmentId = 1,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Question
                {
                    Id = 2,
                    QuestionText = "Which of the following is a programming language?",
                    OptionA = "HTML",
                    OptionB = "CSS",
                    OptionC = "JavaScript",
                    OptionD = "All of the above",
                    CorrectAnswer = "D",
                    AssignmentId = 1,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Question
                {
                    Id = 3,
                    QuestionText = "What does 'if' statement do?",
                    OptionA = "Loops through code",
                    OptionB = "Makes decisions in code",
                    OptionC = "Stores data",
                    OptionD = "Prints output",
                    CorrectAnswer = "B",
                    AssignmentId = 1,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Question
                {
                    Id = 4,
                    QuestionText = "What is a function?",
                    OptionA = "A reusable block of code",
                    OptionB = "A variable",
                    OptionC = "A loop",
                    OptionD = "A condition",
                    CorrectAnswer = "A",
                    AssignmentId = 1,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Question
                {
                    Id = 5,
                    QuestionText = "What does debugging mean?",
                    OptionA = "Writing code",
                    OptionB = "Testing code",
                    OptionC = "Finding and fixing errors",
                    OptionD = "Compiling code",
                    CorrectAnswer = "C",
                    AssignmentId = 1,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            );

            // Random assignment progress for learners
            var random = new Random(42); // Fixed seed for consistent results
            var baseDate = DateTime.UtcNow.AddDays(-5);

            // Create assignment progress for some learners (randomly)
            var assignmentProgresses = new List<AssignmentProgress>();
            var userAnswers = new List<UserAnswer>();

            // Learners 2-11 (learner2-learner10)
            for (int userId = 3; userId <= 11; userId++)
            {
                // 70% chance to have started the assignment
                if (random.NextDouble() < 0.7)
                {
                    var startedAt = baseDate.AddHours(random.Next(0, 120)); // Random start time in last 5 days
                    var isCompleted = random.NextDouble() < 0.8; // 80% completion rate among those who started

                    var progress = new AssignmentProgress
                    {
                        Id = userId - 2, // Starting from ID 2 (learner1 already has ID 1)
                        UserId = userId,
                        AssignmentId = 1,
                        StartedAt = startedAt,
                        IsCompleted = isCompleted,
                        UpdatedAt = isCompleted ? startedAt.AddMinutes(random.Next(15, 60)) : startedAt.AddMinutes(random.Next(5, 30))
                    };

                    if (isCompleted)
                    {
                        progress.CompletedAt = progress.UpdatedAt;

                        // Generate random answers and calculate score
                        var correctAnswers = 0;
                        var questionAnswers = new string[] { "B", "D", "B", "A", "C" }; // Correct answers

                        for (int questionId = 1; questionId <= 5; questionId++)
                        {
                            var options = new string[] { "A", "B", "C", "D" };
                            var selectedAnswer = options[random.Next(4)];

                            // Bias towards correct answers (60% chance)
                            if (random.NextDouble() < 0.6)
                            {
                                selectedAnswer = questionAnswers[questionId - 1];
                            }

                            var isCorrect = selectedAnswer == questionAnswers[questionId - 1];
                            if (isCorrect) correctAnswers++;

                            userAnswers.Add(new UserAnswer
                            {
                                Id = ((userId - 3) * 5) + questionId + 5, // Unique ID calculation
                                UserId = userId,
                                QuestionId = questionId,
                                AssignmentProgressId = progress.Id,
                                SelectedAnswer = selectedAnswer,
                                IsCorrect = isCorrect,
                                AnsweredAt = progress.StartedAt.AddMinutes(random.Next(5, 45))
                            });
                        }

                        progress.Score = (correctAnswers * 100) / 5;
                    }

                    assignmentProgresses.Add(progress);
                }
            }

            // Add the assignment progress data
            modelBuilder.Entity<AssignmentProgress>().HasData(assignmentProgresses.ToArray());

            // Add the user answers data
            modelBuilder.Entity<UserAnswer>().HasData(userAnswers.ToArray());
        }
    }
}
