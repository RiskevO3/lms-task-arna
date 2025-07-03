using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace lmsarnatask.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateSqlServer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ManagerId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaterialContent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaterialType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedById = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    AssignmentId = table.Column<int>(type: "int", nullable: false),
                    Score = table.Column<int>(type: "int", nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StartedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OptionA = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OptionB = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OptionC = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OptionD = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CorrectAnswer = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    AssignmentId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    AssignmentProgressId = table.Column<int>(type: "int", nullable: false),
                    SelectedAnswer = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    IsCorrect = table.Column<bool>(type: "bit", nullable: false),
                    AnsweredAt = table.Column<DateTime>(type: "datetime2", nullable: false)
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
                        onDelete: ReferentialAction.Restrict);
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
                values: new object[] { 1, new DateTime(2025, 7, 3, 10, 49, 3, 963, DateTimeKind.Utc).AddTicks(4960), "manager@example.com", null, "$2a$11$8VugUlXPoBaMzcN5GTEjOOa7V8ErGXChv799qrBVBa3aWR/pmSzui", "Manager", new DateTime(2025, 7, 3, 10, 49, 3, 963, DateTimeKind.Utc).AddTicks(4960), "manager" });

            migrationBuilder.InsertData(
                table: "Assignments",
                columns: new[] { "Id", "CreatedAt", "CreatedById", "Description", "EndDate", "IsActive", "MaterialContent", "MaterialType", "StartDate", "Title", "UpdatedAt" },
                values: new object[] { 1, new DateTime(2025, 7, 3, 10, 49, 5, 305, DateTimeKind.Utc).AddTicks(1050), 1, "Learn the basics of programming concepts", new DateTime(2025, 7, 10, 10, 49, 5, 305, DateTimeKind.Utc).AddTicks(1050), true, "https://example.com/programming-basics.pdf", "PDF", new DateTime(2025, 6, 26, 10, 49, 5, 305, DateTimeKind.Utc).AddTicks(1030), "Introduction to Programming", new DateTime(2025, 7, 3, 10, 49, 5, 305, DateTimeKind.Utc).AddTicks(1050) });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "ManagerId", "PasswordHash", "Role", "UpdatedAt", "Username" },
                values: new object[,]
                {
                    { 2, new DateTime(2025, 7, 3, 10, 49, 4, 96, DateTimeKind.Utc).AddTicks(160), "learner1@example.com", 1, "$2a$11$aRD8sJ/9C479v9fk0d8Amuon/lw7ga4k30/hsxN94aHALo96sb3M6", "Learner", new DateTime(2025, 7, 3, 10, 49, 4, 96, DateTimeKind.Utc).AddTicks(160), "learner1" },
                    { 3, new DateTime(2025, 7, 3, 10, 49, 4, 229, DateTimeKind.Utc).AddTicks(4520), "learner2@example.com", 1, "$2a$11$44xhAay3vTQq8LeAsaWJtO/.EAB5GN9AGARSLx6Yzv5HxKBmx0Vcq", "Learner", new DateTime(2025, 7, 3, 10, 49, 4, 229, DateTimeKind.Utc).AddTicks(4520), "learner2" },
                    { 4, new DateTime(2025, 7, 3, 10, 49, 4, 361, DateTimeKind.Utc).AddTicks(2750), "learner3@example.com", 1, "$2a$11$AAqbEKY830pfZSdN6/QosuQ4ITey0WJcTHEC2qslK1iQVzuI10Qua", "Learner", new DateTime(2025, 7, 3, 10, 49, 4, 361, DateTimeKind.Utc).AddTicks(2750), "learner3" },
                    { 5, new DateTime(2025, 7, 3, 10, 49, 4, 492, DateTimeKind.Utc).AddTicks(2470), "learner4@example.com", 1, "$2a$11$jhktvaF2o5TspPhdezXWSOZ8oxyhK.nLIjCHAu96LMppEnN9khICS", "Learner", new DateTime(2025, 7, 3, 10, 49, 4, 492, DateTimeKind.Utc).AddTicks(2470), "learner4" },
                    { 6, new DateTime(2025, 7, 3, 10, 49, 4, 635, DateTimeKind.Utc).AddTicks(1380), "learner5@example.com", 1, "$2a$11$jzdDf4qItqgcLbKi4QYn9en0sVMGarElUV84CdxQKRUEFdHpeWM26", "Learner", new DateTime(2025, 7, 3, 10, 49, 4, 635, DateTimeKind.Utc).AddTicks(1380), "learner5" },
                    { 7, new DateTime(2025, 7, 3, 10, 49, 4, 768, DateTimeKind.Utc).AddTicks(1000), "learner6@example.com", 1, "$2a$11$1kZjZsS5tq5VY5qalhjcUu186Fv8T2e.zQyQ8yHR7EJZ5xm3FLLg2", "Learner", new DateTime(2025, 7, 3, 10, 49, 4, 768, DateTimeKind.Utc).AddTicks(1000), "learner6" },
                    { 8, new DateTime(2025, 7, 3, 10, 49, 4, 900, DateTimeKind.Utc).AddTicks(5270), "learner7@example.com", 1, "$2a$11$.vKjd9//OciJEPToE.SxUeWbvuj3aY4KqPZrotcyTISaE7iRae.O2", "Learner", new DateTime(2025, 7, 3, 10, 49, 4, 900, DateTimeKind.Utc).AddTicks(5270), "learner7" },
                    { 9, new DateTime(2025, 7, 3, 10, 49, 5, 36, DateTimeKind.Utc).AddTicks(5220), "learner8@example.com", 1, "$2a$11$RzeEAWRfjsLxMd78NzGi7.wVlrOg2Vaklam/87BEnXvQLpHojVfmq", "Learner", new DateTime(2025, 7, 3, 10, 49, 5, 36, DateTimeKind.Utc).AddTicks(5220), "learner8" },
                    { 10, new DateTime(2025, 7, 3, 10, 49, 5, 170, DateTimeKind.Utc).AddTicks(1240), "learner9@example.com", 1, "$2a$11$YKlo11VuJMhClK3fJYVNOuvF6sRArZTacalxroyB92bUap3D0UF9.", "Learner", new DateTime(2025, 7, 3, 10, 49, 5, 170, DateTimeKind.Utc).AddTicks(1240), "learner9" },
                    { 11, new DateTime(2025, 7, 3, 10, 49, 5, 304, DateTimeKind.Utc).AddTicks(9400), "learner10@example.com", 1, "$2a$11$WhppKAlHk1nrQe5T4GaX9.uka2LlCefsiIiFZeM00miC0y6Up0kNy", "Learner", new DateTime(2025, 7, 3, 10, 49, 5, 304, DateTimeKind.Utc).AddTicks(9400), "learner10" }
                });

            migrationBuilder.InsertData(
                table: "AssignmentProgresses",
                columns: new[] { "Id", "AssignmentId", "CompletedAt", "IsCompleted", "Score", "StartedAt", "UpdatedAt", "UserId" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2025, 6, 29, 3, 27, 5, 305, DateTimeKind.Utc).AddTicks(1300), true, 100, new DateTime(2025, 6, 29, 2, 49, 5, 305, DateTimeKind.Utc).AddTicks(1300), new DateTime(2025, 6, 29, 3, 27, 5, 305, DateTimeKind.Utc).AddTicks(1300), 3 },
                    { 2, 1, new DateTime(2025, 6, 30, 10, 8, 5, 305, DateTimeKind.Utc).AddTicks(1300), true, 40, new DateTime(2025, 6, 30, 9, 49, 5, 305, DateTimeKind.Utc).AddTicks(1300), new DateTime(2025, 6, 30, 10, 8, 5, 305, DateTimeKind.Utc).AddTicks(1300), 4 },
                    { 3, 1, new DateTime(2025, 7, 2, 11, 21, 5, 305, DateTimeKind.Utc).AddTicks(1300), true, 80, new DateTime(2025, 7, 2, 10, 49, 5, 305, DateTimeKind.Utc).AddTicks(1300), new DateTime(2025, 7, 2, 11, 21, 5, 305, DateTimeKind.Utc).AddTicks(1300), 5 },
                    { 4, 1, new DateTime(2025, 7, 1, 23, 13, 5, 305, DateTimeKind.Utc).AddTicks(1300), true, 60, new DateTime(2025, 7, 1, 22, 49, 5, 305, DateTimeKind.Utc).AddTicks(1300), new DateTime(2025, 7, 1, 23, 13, 5, 305, DateTimeKind.Utc).AddTicks(1300), 6 },
                    { 5, 1, new DateTime(2025, 6, 28, 18, 5, 5, 305, DateTimeKind.Utc).AddTicks(1300), true, 40, new DateTime(2025, 6, 28, 17, 49, 5, 305, DateTimeKind.Utc).AddTicks(1300), new DateTime(2025, 6, 28, 18, 5, 5, 305, DateTimeKind.Utc).AddTicks(1300), 7 },
                    { 7, 1, null, false, 0, new DateTime(2025, 7, 2, 19, 49, 5, 305, DateTimeKind.Utc).AddTicks(1300), new DateTime(2025, 7, 2, 20, 2, 5, 305, DateTimeKind.Utc).AddTicks(1300), 9 },
                    { 9, 1, new DateTime(2025, 6, 28, 23, 18, 5, 305, DateTimeKind.Utc).AddTicks(1300), true, 60, new DateTime(2025, 6, 28, 22, 49, 5, 305, DateTimeKind.Utc).AddTicks(1300), new DateTime(2025, 6, 28, 23, 18, 5, 305, DateTimeKind.Utc).AddTicks(1300), 11 }
                });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "AssignmentId", "CorrectAnswer", "CreatedAt", "OptionA", "OptionB", "OptionC", "OptionD", "QuestionText", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, 1, "B", new DateTime(2025, 7, 3, 10, 49, 5, 305, DateTimeKind.Utc).AddTicks(1160), "A fixed value", "A container for storing data", "A function", "A loop", "What is a variable in programming?", new DateTime(2025, 7, 3, 10, 49, 5, 305, DateTimeKind.Utc).AddTicks(1160) },
                    { 2, 1, "D", new DateTime(2025, 7, 3, 10, 49, 5, 305, DateTimeKind.Utc).AddTicks(1160), "HTML", "CSS", "JavaScript", "All of the above", "Which of the following is a programming language?", new DateTime(2025, 7, 3, 10, 49, 5, 305, DateTimeKind.Utc).AddTicks(1160) },
                    { 3, 1, "B", new DateTime(2025, 7, 3, 10, 49, 5, 305, DateTimeKind.Utc).AddTicks(1160), "Loops through code", "Makes decisions in code", "Stores data", "Prints output", "What does 'if' statement do?", new DateTime(2025, 7, 3, 10, 49, 5, 305, DateTimeKind.Utc).AddTicks(1160) },
                    { 4, 1, "A", new DateTime(2025, 7, 3, 10, 49, 5, 305, DateTimeKind.Utc).AddTicks(1170), "A reusable block of code", "A variable", "A loop", "A condition", "What is a function?", new DateTime(2025, 7, 3, 10, 49, 5, 305, DateTimeKind.Utc).AddTicks(1170) },
                    { 5, 1, "C", new DateTime(2025, 7, 3, 10, 49, 5, 305, DateTimeKind.Utc).AddTicks(1170), "Writing code", "Testing code", "Finding and fixing errors", "Compiling code", "What does debugging mean?", new DateTime(2025, 7, 3, 10, 49, 5, 305, DateTimeKind.Utc).AddTicks(1170) }
                });

            migrationBuilder.InsertData(
                table: "UserAnswers",
                columns: new[] { "Id", "AnsweredAt", "AssignmentProgressId", "IsCorrect", "QuestionId", "SelectedAnswer", "UserId" },
                values: new object[,]
                {
                    { 6, new DateTime(2025, 6, 29, 3, 22, 5, 305, DateTimeKind.Utc).AddTicks(1300), 1, true, 1, "B", 3 },
                    { 7, new DateTime(2025, 6, 29, 3, 24, 5, 305, DateTimeKind.Utc).AddTicks(1300), 1, true, 2, "D", 3 },
                    { 8, new DateTime(2025, 6, 29, 3, 14, 5, 305, DateTimeKind.Utc).AddTicks(1300), 1, true, 3, "B", 3 },
                    { 9, new DateTime(2025, 6, 29, 3, 4, 5, 305, DateTimeKind.Utc).AddTicks(1300), 1, true, 4, "A", 3 },
                    { 10, new DateTime(2025, 6, 29, 3, 26, 5, 305, DateTimeKind.Utc).AddTicks(1300), 1, true, 5, "C", 3 },
                    { 11, new DateTime(2025, 6, 30, 10, 15, 5, 305, DateTimeKind.Utc).AddTicks(1300), 2, false, 1, "C", 4 },
                    { 12, new DateTime(2025, 6, 30, 9, 59, 5, 305, DateTimeKind.Utc).AddTicks(1300), 2, false, 2, "A", 4 },
                    { 13, new DateTime(2025, 6, 30, 10, 14, 5, 305, DateTimeKind.Utc).AddTicks(1300), 2, false, 3, "D", 4 },
                    { 14, new DateTime(2025, 6, 30, 10, 6, 5, 305, DateTimeKind.Utc).AddTicks(1300), 2, true, 4, "A", 4 },
                    { 15, new DateTime(2025, 6, 30, 9, 54, 5, 305, DateTimeKind.Utc).AddTicks(1300), 2, true, 5, "C", 4 },
                    { 16, new DateTime(2025, 7, 2, 10, 56, 5, 305, DateTimeKind.Utc).AddTicks(1300), 3, true, 1, "B", 5 },
                    { 17, new DateTime(2025, 7, 2, 10, 58, 5, 305, DateTimeKind.Utc).AddTicks(1300), 3, true, 2, "D", 5 },
                    { 18, new DateTime(2025, 7, 2, 11, 5, 5, 305, DateTimeKind.Utc).AddTicks(1300), 3, false, 3, "A", 5 },
                    { 19, new DateTime(2025, 7, 2, 10, 55, 5, 305, DateTimeKind.Utc).AddTicks(1300), 3, true, 4, "A", 5 },
                    { 20, new DateTime(2025, 7, 2, 10, 56, 5, 305, DateTimeKind.Utc).AddTicks(1300), 3, true, 5, "C", 5 },
                    { 21, new DateTime(2025, 7, 1, 23, 4, 5, 305, DateTimeKind.Utc).AddTicks(1300), 4, true, 1, "B", 6 },
                    { 22, new DateTime(2025, 7, 1, 22, 56, 5, 305, DateTimeKind.Utc).AddTicks(1300), 4, false, 2, "A", 6 },
                    { 23, new DateTime(2025, 7, 1, 23, 25, 5, 305, DateTimeKind.Utc).AddTicks(1300), 4, true, 3, "B", 6 },
                    { 24, new DateTime(2025, 7, 1, 23, 13, 5, 305, DateTimeKind.Utc).AddTicks(1300), 4, true, 4, "A", 6 },
                    { 25, new DateTime(2025, 7, 1, 22, 54, 5, 305, DateTimeKind.Utc).AddTicks(1300), 4, false, 5, "A", 6 },
                    { 26, new DateTime(2025, 6, 28, 18, 12, 5, 305, DateTimeKind.Utc).AddTicks(1300), 5, false, 1, "D", 7 },
                    { 27, new DateTime(2025, 6, 28, 17, 55, 5, 305, DateTimeKind.Utc).AddTicks(1300), 5, false, 2, "B", 7 },
                    { 28, new DateTime(2025, 6, 28, 18, 11, 5, 305, DateTimeKind.Utc).AddTicks(1300), 5, true, 3, "B", 7 },
                    { 29, new DateTime(2025, 6, 28, 18, 10, 5, 305, DateTimeKind.Utc).AddTicks(1300), 5, true, 4, "A", 7 },
                    { 30, new DateTime(2025, 6, 28, 18, 17, 5, 305, DateTimeKind.Utc).AddTicks(1300), 5, false, 5, "B", 7 },
                    { 46, new DateTime(2025, 6, 28, 22, 59, 5, 305, DateTimeKind.Utc).AddTicks(1300), 9, true, 1, "B", 11 },
                    { 47, new DateTime(2025, 6, 28, 23, 14, 5, 305, DateTimeKind.Utc).AddTicks(1300), 9, true, 2, "D", 11 },
                    { 48, new DateTime(2025, 6, 28, 23, 20, 5, 305, DateTimeKind.Utc).AddTicks(1300), 9, false, 3, "D", 11 },
                    { 49, new DateTime(2025, 6, 28, 23, 16, 5, 305, DateTimeKind.Utc).AddTicks(1300), 9, true, 4, "A", 11 },
                    { 50, new DateTime(2025, 6, 28, 22, 54, 5, 305, DateTimeKind.Utc).AddTicks(1300), 9, false, 5, "D", 11 }
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
