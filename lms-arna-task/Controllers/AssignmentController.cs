using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using lms_arna_task.Services.Interfaces;
using System.Security.Claims;

namespace lms_arna_task.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AssignmentController : ControllerBase
    {
        private readonly IAssignmentService _assignmentService;

        public AssignmentController(IAssignmentService assignmentService)
        {
            _assignmentService = assignmentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetActiveAssignments()
        {
            var assignments = await _assignmentService.GetActiveAssignmentsAsync();
            return Ok(assignments);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAssignment(int id)
        {
            var assignment = await _assignmentService.GetAssignmentByIdAsync(id);
            if (assignment == null)
            {
                return NotFound();
            }

            return Ok(assignment);
        }

        [HttpGet("{id}/with-questions")]
        public async Task<IActionResult> GetAssignmentWithQuestions(int id)
        {
            var assignment = await _assignmentService.GetAssignmentWithQuestionsAsync(id);
            if (assignment == null)
            {
                return NotFound();
            }

            return Ok(assignment);
        }
    }
}
