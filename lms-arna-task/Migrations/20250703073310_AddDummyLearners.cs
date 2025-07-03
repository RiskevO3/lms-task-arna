using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace lmsarnatask.Migrations
{
    /// <inheritdoc />
    public partial class AddDummyLearners : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Username = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Role = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    ManagerId = table.Column<int>(type: "INTEGER", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Users_ManagerId",
                        column: x => x.ManagerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Assignments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    MaterialContent = table.Column<string>(type: "TEXT", nullable: false),
                    MaterialType = table.Column<string>(type: "TEXT", nullable: false),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedById = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Assignments_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AssignmentProgresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    AssignmentId = table.Column<int>(type: "INTEGER", nullable: false),
                    Score = table.Column<int>(type: "INTEGER", nullable: false),
                    IsCompleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    StartedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssignmentProgresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssignmentProgresses_Assignments_AssignmentId",
                        column: x => x.AssignmentId,
                        principalTable: "Assignments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssignmentProgresses_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    QuestionText = table.Column<string>(type: "TEXT", nullable: false),
                    OptionA = table.Column<string>(type: "TEXT", nullable: false),
                    OptionB = table.Column<string>(type: "TEXT", nullable: false),
                    OptionC = table.Column<string>(type: "TEXT", nullable: false),
                    OptionD = table.Column<string>(type: "TEXT", nullable: false),
                    CorrectAnswer = table.Column<string>(type: "TEXT", maxLength: 1, nullable: false),
                    AssignmentId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Questions_Assignments_AssignmentId",
                        column: x => x.AssignmentId,
                        principalTable: "Assignments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserAnswers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    QuestionId = table.Column<int>(type: "INTEGER", nullable: false),
                    AssignmentProgressId = table.Column<int>(type: "INTEGER", nullable: false),
                    SelectedAnswer = table.Column<string>(type: "TEXT", maxLength: 1, nullable: false),
                    IsCorrect = table.Column<bool>(type: "INTEGER", nullable: false),
                    AnsweredAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAnswers_AssignmentProgresses_AssignmentProgressId",
                        column: x => x.AssignmentProgressId,
                        principalTable: "AssignmentProgresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserAnswers_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserAnswers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "ManagerId", "PasswordHash", "Role", "UpdatedAt", "Username" },
                values: new object[] { 1, new DateTime(2025, 7, 3, 7, 33, 8, 888, DateTimeKind.Utc).AddTicks(6750), "manager@example.com", null, "$2a$11$aTrhKBJ8IgLzSHYLBpNiWOldfBrJDRymED1OnMQ2ukQ6oSfP9.3rq", "Manager", new DateTime(2025, 7, 3, 7, 33, 8, 888, DateTimeKind.Utc).AddTicks(6750), "manager" });

            migrationBuilder.InsertData(
                table: "Assignments",
                columns: new[] { "Id", "CreatedAt", "CreatedById", "Description", "EndDate", "IsActive", "MaterialContent", "MaterialType", "StartDate", "Title", "UpdatedAt" },
                values: new object[] { 1, new DateTime(2025, 7, 3, 7, 33, 10, 204, DateTimeKind.Utc).AddTicks(2240), 1, "Learn the basics of programming concepts", new DateTime(2025, 7, 10, 7, 33, 10, 204, DateTimeKind.Utc).AddTicks(2240), true, "https://example.com/programming-basics.pdf", "PDF", new DateTime(2025, 6, 26, 7, 33, 10, 204, DateTimeKind.Utc).AddTicks(2220), "Introduction to Programming", new DateTime(2025, 7, 3, 7, 33, 10, 204, DateTimeKind.Utc).AddTicks(2240) });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "ManagerId", "PasswordHash", "Role", "UpdatedAt", "Username" },
                values: new object[,]
                {
                    { 2, new DateTime(2025, 7, 3, 7, 33, 9, 20, DateTimeKind.Utc).AddTicks(9370), "learner1@example.com", 1, "$2a$11$Z8P6LGLvh5rDkNQCDzehLeqEWQ32sgpyjtXA/vRPMAq1shIpA9D7i", "Learner", new DateTime(2025, 7, 3, 7, 33, 9, 20, DateTimeKind.Utc).AddTicks(9370), "learner1" },
                    { 3, new DateTime(2025, 7, 3, 7, 33, 9, 154, DateTimeKind.Utc).AddTicks(5450), "learner2@example.com", 1, "$2a$11$1VBPWO21Eb2s0nto5gbuxuue3TO5EJZ20zH6TFr7TrDDHUt9TQHaW", "Learner", new DateTime(2025, 7, 3, 7, 33, 9, 154, DateTimeKind.Utc).AddTicks(5450), "learner2" },
                    { 4, new DateTime(2025, 7, 3, 7, 33, 9, 287, DateTimeKind.Utc).AddTicks(6260), "learner3@example.com", 1, "$2a$11$1PKU/mneyvrhNZcA80DPpeJWoAvNf8okI75RxnDOU/fEAUJV9DzN6", "Learner", new DateTime(2025, 7, 3, 7, 33, 9, 287, DateTimeKind.Utc).AddTicks(6260), "learner3" },
                    { 5, new DateTime(2025, 7, 3, 7, 33, 9, 418, DateTimeKind.Utc).AddTicks(5400), "learner4@example.com", 1, "$2a$11$rhcvtQDUFkzaRxEDGPnSUeNLQPQuNVrgy/tKQAh/ZEaZMGdiw9Iau", "Learner", new DateTime(2025, 7, 3, 7, 33, 9, 418, DateTimeKind.Utc).AddTicks(5400), "learner4" },
                    { 6, new DateTime(2025, 7, 3, 7, 33, 9, 549, DateTimeKind.Utc).AddTicks(380), "learner5@example.com", 1, "$2a$11$njegO8Tt5EOP6CS/OCPpQ.w7bR/g4Qz6JnVdFtC88liA4oTuqe9dG", "Learner", new DateTime(2025, 7, 3, 7, 33, 9, 549, DateTimeKind.Utc).AddTicks(380), "learner5" },
                    { 7, new DateTime(2025, 7, 3, 7, 33, 9, 680, DateTimeKind.Utc).AddTicks(5310), "learner6@example.com", 1, "$2a$11$pf4k3Qqaz3lFXFo5RW1NfuTgw8qh9sqHJB87KKFZ0ngEYOX3OSF.K", "Learner", new DateTime(2025, 7, 3, 7, 33, 9, 680, DateTimeKind.Utc).AddTicks(5310), "learner6" },
                    { 8, new DateTime(2025, 7, 3, 7, 33, 9, 811, DateTimeKind.Utc).AddTicks(2940), "learner7@example.com", 1, "$2a$11$81ySK6jm3wUT2BLhUHEWtuFSF2eXz8UQxsOVAPZiz5Z6ryJwUtrsi", "Learner", new DateTime(2025, 7, 3, 7, 33, 9, 811, DateTimeKind.Utc).AddTicks(2940), "learner7" },
                    { 9, new DateTime(2025, 7, 3, 7, 33, 9, 942, DateTimeKind.Utc).AddTicks(200), "learner8@example.com", 1, "$2a$11$7xrnB5itcjd2rphCyUHrj.eAEIsuNVPqdLS/En6yRCAYXB6qqrE.q", "Learner", new DateTime(2025, 7, 3, 7, 33, 9, 942, DateTimeKind.Utc).AddTicks(200), "learner8" },
                    { 10, new DateTime(2025, 7, 3, 7, 33, 10, 71, DateTimeKind.Utc).AddTicks(6970), "learner9@example.com", 1, "$2a$11$zqzbZQUCRWs4t9.DG01O9uedorOlaPCFMKbA9fLUlTUFE7Odmx.IK", "Learner", new DateTime(2025, 7, 3, 7, 33, 10, 71, DateTimeKind.Utc).AddTicks(6970), "learner9" },
                    { 11, new DateTime(2025, 7, 3, 7, 33, 10, 204, DateTimeKind.Utc).AddTicks(1200), "learner10@example.com", 1, "$2a$11$B0OfveVG51mAOSbkhja67OZRFXKZC267KKmcLmAmgTLGYyb8UnI/u", "Learner", new DateTime(2025, 7, 3, 7, 33, 10, 204, DateTimeKind.Utc).AddTicks(1200), "learner10" }
                });

            migrationBuilder.InsertData(
                table: "AssignmentProgresses",
                columns: new[] { "Id", "AssignmentId", "CompletedAt", "IsCompleted", "Score", "StartedAt", "UpdatedAt", "UserId" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2025, 6, 29, 0, 11, 10, 204, DateTimeKind.Utc).AddTicks(2450), true, 100, new DateTime(2025, 6, 28, 23, 33, 10, 204, DateTimeKind.Utc).AddTicks(2450), new DateTime(2025, 6, 29, 0, 11, 10, 204, DateTimeKind.Utc).AddTicks(2450), 3 },
                    { 2, 1, new DateTime(2025, 6, 30, 6, 52, 10, 204, DateTimeKind.Utc).AddTicks(2450), true, 40, new DateTime(2025, 6, 30, 6, 33, 10, 204, DateTimeKind.Utc).AddTicks(2450), new DateTime(2025, 6, 30, 6, 52, 10, 204, DateTimeKind.Utc).AddTicks(2450), 4 },
                    { 3, 1, new DateTime(2025, 7, 2, 8, 5, 10, 204, DateTimeKind.Utc).AddTicks(2450), true, 80, new DateTime(2025, 7, 2, 7, 33, 10, 204, DateTimeKind.Utc).AddTicks(2450), new DateTime(2025, 7, 2, 8, 5, 10, 204, DateTimeKind.Utc).AddTicks(2450), 5 },
                    { 4, 1, new DateTime(2025, 7, 1, 19, 57, 10, 204, DateTimeKind.Utc).AddTicks(2450), true, 60, new DateTime(2025, 7, 1, 19, 33, 10, 204, DateTimeKind.Utc).AddTicks(2450), new DateTime(2025, 7, 1, 19, 57, 10, 204, DateTimeKind.Utc).AddTicks(2450), 6 },
                    { 5, 1, new DateTime(2025, 6, 28, 14, 49, 10, 204, DateTimeKind.Utc).AddTicks(2450), true, 40, new DateTime(2025, 6, 28, 14, 33, 10, 204, DateTimeKind.Utc).AddTicks(2450), new DateTime(2025, 6, 28, 14, 49, 10, 204, DateTimeKind.Utc).AddTicks(2450), 7 },
                    { 7, 1, null, false, 0, new DateTime(2025, 7, 2, 16, 33, 10, 204, DateTimeKind.Utc).AddTicks(2450), new DateTime(2025, 7, 2, 16, 46, 10, 204, DateTimeKind.Utc).AddTicks(2450), 9 },
                    { 9, 1, new DateTime(2025, 6, 28, 20, 2, 10, 204, DateTimeKind.Utc).AddTicks(2450), true, 60, new DateTime(2025, 6, 28, 19, 33, 10, 204, DateTimeKind.Utc).AddTicks(2450), new DateTime(2025, 6, 28, 20, 2, 10, 204, DateTimeKind.Utc).AddTicks(2450), 11 }
                });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "AssignmentId", "CorrectAnswer", "CreatedAt", "OptionA", "OptionB", "OptionC", "OptionD", "QuestionText", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, 1, "B", new DateTime(2025, 7, 3, 7, 33, 10, 204, DateTimeKind.Utc).AddTicks(2340), "A fixed value", "A container for storing data", "A function", "A loop", "What is a variable in programming?", new DateTime(2025, 7, 3, 7, 33, 10, 204, DateTimeKind.Utc).AddTicks(2340) },
                    { 2, 1, "D", new DateTime(2025, 7, 3, 7, 33, 10, 204, DateTimeKind.Utc).AddTicks(2350), "HTML", "CSS", "JavaScript", "All of the above", "Which of the following is a programming language?", new DateTime(2025, 7, 3, 7, 33, 10, 204, DateTimeKind.Utc).AddTicks(2350) },
                    { 3, 1, "B", new DateTime(2025, 7, 3, 7, 33, 10, 204, DateTimeKind.Utc).AddTicks(2350), "Loops through code", "Makes decisions in code", "Stores data", "Prints output", "What does 'if' statement do?", new DateTime(2025, 7, 3, 7, 33, 10, 204, DateTimeKind.Utc).AddTicks(2350) },
                    { 4, 1, "A", new DateTime(2025, 7, 3, 7, 33, 10, 204, DateTimeKind.Utc).AddTicks(2350), "A reusable block of code", "A variable", "A loop", "A condition", "What is a function?", new DateTime(2025, 7, 3, 7, 33, 10, 204, DateTimeKind.Utc).AddTicks(2350) },
                    { 5, 1, "C", new DateTime(2025, 7, 3, 7, 33, 10, 204, DateTimeKind.Utc).AddTicks(2350), "Writing code", "Testing code", "Finding and fixing errors", "Compiling code", "What does debugging mean?", new DateTime(2025, 7, 3, 7, 33, 10, 204, DateTimeKind.Utc).AddTicks(2350) }
                });

            migrationBuilder.InsertData(
                table: "UserAnswers",
                columns: new[] { "Id", "AnsweredAt", "AssignmentProgressId", "IsCorrect", "QuestionId", "SelectedAnswer", "UserId" },
                values: new object[,]
                {
                    { 6, new DateTime(2025, 6, 29, 0, 6, 10, 204, DateTimeKind.Utc).AddTicks(2450), 1, true, 1, "B", 3 },
                    { 7, new DateTime(2025, 6, 29, 0, 8, 10, 204, DateTimeKind.Utc).AddTicks(2450), 1, true, 2, "D", 3 },
                    { 8, new DateTime(2025, 6, 28, 23, 58, 10, 204, DateTimeKind.Utc).AddTicks(2450), 1, true, 3, "B", 3 },
                    { 9, new DateTime(2025, 6, 28, 23, 48, 10, 204, DateTimeKind.Utc).AddTicks(2450), 1, true, 4, "A", 3 },
                    { 10, new DateTime(2025, 6, 29, 0, 10, 10, 204, DateTimeKind.Utc).AddTicks(2450), 1, true, 5, "C", 3 },
                    { 11, new DateTime(2025, 6, 30, 6, 59, 10, 204, DateTimeKind.Utc).AddTicks(2450), 2, false, 1, "C", 4 },
                    { 12, new DateTime(2025, 6, 30, 6, 43, 10, 204, DateTimeKind.Utc).AddTicks(2450), 2, false, 2, "A", 4 },
                    { 13, new DateTime(2025, 6, 30, 6, 58, 10, 204, DateTimeKind.Utc).AddTicks(2450), 2, false, 3, "D", 4 },
                    { 14, new DateTime(2025, 6, 30, 6, 50, 10, 204, DateTimeKind.Utc).AddTicks(2450), 2, true, 4, "A", 4 },
                    { 15, new DateTime(2025, 6, 30, 6, 38, 10, 204, DateTimeKind.Utc).AddTicks(2450), 2, true, 5, "C", 4 },
                    { 16, new DateTime(2025, 7, 2, 7, 40, 10, 204, DateTimeKind.Utc).AddTicks(2450), 3, true, 1, "B", 5 },
                    { 17, new DateTime(2025, 7, 2, 7, 42, 10, 204, DateTimeKind.Utc).AddTicks(2450), 3, true, 2, "D", 5 },
                    { 18, new DateTime(2025, 7, 2, 7, 49, 10, 204, DateTimeKind.Utc).AddTicks(2450), 3, false, 3, "A", 5 },
                    { 19, new DateTime(2025, 7, 2, 7, 39, 10, 204, DateTimeKind.Utc).AddTicks(2450), 3, true, 4, "A", 5 },
                    { 20, new DateTime(2025, 7, 2, 7, 40, 10, 204, DateTimeKind.Utc).AddTicks(2450), 3, true, 5, "C", 5 },
                    { 21, new DateTime(2025, 7, 1, 19, 48, 10, 204, DateTimeKind.Utc).AddTicks(2450), 4, true, 1, "B", 6 },
                    { 22, new DateTime(2025, 7, 1, 19, 40, 10, 204, DateTimeKind.Utc).AddTicks(2450), 4, false, 2, "A", 6 },
                    { 23, new DateTime(2025, 7, 1, 20, 9, 10, 204, DateTimeKind.Utc).AddTicks(2450), 4, true, 3, "B", 6 },
                    { 24, new DateTime(2025, 7, 1, 19, 57, 10, 204, DateTimeKind.Utc).AddTicks(2450), 4, true, 4, "A", 6 },
                    { 25, new DateTime(2025, 7, 1, 19, 38, 10, 204, DateTimeKind.Utc).AddTicks(2450), 4, false, 5, "A", 6 },
                    { 26, new DateTime(2025, 6, 28, 14, 56, 10, 204, DateTimeKind.Utc).AddTicks(2450), 5, false, 1, "D", 7 },
                    { 27, new DateTime(2025, 6, 28, 14, 39, 10, 204, DateTimeKind.Utc).AddTicks(2450), 5, false, 2, "B", 7 },
                    { 28, new DateTime(2025, 6, 28, 14, 55, 10, 204, DateTimeKind.Utc).AddTicks(2450), 5, true, 3, "B", 7 },
                    { 29, new DateTime(2025, 6, 28, 14, 54, 10, 204, DateTimeKind.Utc).AddTicks(2450), 5, true, 4, "A", 7 },
                    { 30, new DateTime(2025, 6, 28, 15, 1, 10, 204, DateTimeKind.Utc).AddTicks(2450), 5, false, 5, "B", 7 },
                    { 46, new DateTime(2025, 6, 28, 19, 43, 10, 204, DateTimeKind.Utc).AddTicks(2450), 9, true, 1, "B", 11 },
                    { 47, new DateTime(2025, 6, 28, 19, 58, 10, 204, DateTimeKind.Utc).AddTicks(2450), 9, true, 2, "D", 11 },
                    { 48, new DateTime(2025, 6, 28, 20, 4, 10, 204, DateTimeKind.Utc).AddTicks(2450), 9, false, 3, "D", 11 },
                    { 49, new DateTime(2025, 6, 28, 20, 0, 10, 204, DateTimeKind.Utc).AddTicks(2450), 9, true, 4, "A", 11 },
                    { 50, new DateTime(2025, 6, 28, 19, 38, 10, 204, DateTimeKind.Utc).AddTicks(2450), 9, false, 5, "D", 11 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentProgresses_AssignmentId",
                table: "AssignmentProgresses",
                column: "AssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentProgresses_UserId_AssignmentId",
                table: "AssignmentProgresses",
                columns: new[] { "UserId", "AssignmentId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_CreatedById",
                table: "Assignments",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_AssignmentId",
                table: "Questions",
                column: "AssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAnswers_AssignmentProgressId",
                table: "UserAnswers",
                column: "AssignmentProgressId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAnswers_QuestionId",
                table: "UserAnswers",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAnswers_UserId",
                table: "UserAnswers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_ManagerId",
                table: "Users",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserAnswers");

            migrationBuilder.DropTable(
                name: "AssignmentProgresses");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "Assignments");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
