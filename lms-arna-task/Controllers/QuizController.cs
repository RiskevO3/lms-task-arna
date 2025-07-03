using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using lms_arna_task.Services.Interfaces;
using lms_arna_task.DTOs;
using System.Security.Claims;

namespace lms_arna_task.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class QuizController : ControllerBase
    {
        private readonly IQuizService _quizService;

        public QuizController(IQuizService quizService)
        {
            _quizService = quizService;
        }

        [HttpGet("progress/{assignmentId}")]
        public async Task<IActionResult> GetProgress(int assignmentId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return Unauthorized();
            }

            var progress = await _quizService.GetUserProgressAsync(userId, assignmentId);
            return Ok(progress);
        }

        [HttpPost("start/{assignmentId}")]
        public async Task<IActionResult> StartAssignment(int assignmentId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return Unauthorized();
            }

            var progress = await _quizService.StartAssignmentAsync(userId, assignmentId);
            return Ok(progress);
        }

        [HttpPost("submit/{assignmentId}")]
        public async Task<IActionResult> SubmitAnswers(int assignmentId, [FromBody] Dictionary<int, string> answers)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return Unauthorized();
            }

            try
            {
                var progress = await _quizService.SubmitAnswersAsync(userId, assignmentId, answers);
                return Ok(progress);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("report")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetProgressReport()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int managerId))
            {
                return Unauthorized();
            }

            var report = await _quizService.GetProgressReportAsync(managerId);

            // Convert to simplified DTO to avoid circular references
            var reportDto = report.Select(p => new ProgressReport
            {
                Id = p.Id,
                UserId = p.UserId,
                Username = p.User?.Username ?? "Unknown",
                Email = p.User?.Email ?? "unknown@example.com",
                AssignmentId = p.AssignmentId,
                AssignmentTitle = p.Assignment?.Title ?? "Unknown Assignment",
                AssignmentDescription = p.Assignment?.Description ?? "No description",
                Score = p.Score,
                IsCompleted = p.IsCompleted,
                CompletedAt = p.CompletedAt,
                StartedAt = p.StartedAt,
                UpdatedAt = p.UpdatedAt
            }).ToList();

            return Ok(reportDto);
        }

        [HttpGet("check-completion/{assignmentId}")]
        public async Task<IActionResult> CheckCompletion(int assignmentId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return Unauthorized();
            }

            var isCompleted = await _quizService.HasUserCompletedAssignmentAsync(userId, assignmentId);
            return Ok(new { isCompleted });
        }

        [HttpGet("progress/user/{userId}")]
        public async Task<IActionResult> GetUserProgress(int userId)
        {
            var currentUserIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (currentUserIdClaim == null || !int.TryParse(currentUserIdClaim.Value, out int currentUserId))
            {
                return Unauthorized();
            }

            // Users can only view their own progress, unless they're a manager
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            if (currentUserId != userId && userRole != "Manager")
            {
                return Forbid();
            }

            // Get all progress for this user
            var userProgress = await _quizService.GetAllUserProgressAsync(userId);

            return Ok(userProgress);
        }

        [HttpGet("answers/{userId}/{assignmentId}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetUserAnswers(int userId, int assignmentId)
        {
            var currentUserIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (currentUserIdClaim == null || !int.TryParse(currentUserIdClaim.Value, out int managerId))
            {
                return Unauthorized();
            }

            var answerDetails = await _quizService.GetUserAnswerDetailsAsync(userId, assignmentId);
            if (answerDetails == null)
            {
                return NotFound(new { message = "No answers found for this user and assignment" });
            }

            return Ok(answerDetails);
        }
    }
}
