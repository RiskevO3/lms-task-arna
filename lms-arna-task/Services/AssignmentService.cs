using lms_arna_task.Models;
using lms_arna_task.Services.Interfaces;
using lms_arna_task.Repositories.Interfaces;

namespace lms_arna_task.Services
{
    public class AssignmentService : IAssignmentService
    {
        private readonly IAssignmentRepository _assignmentRepository;

        public AssignmentService(IAssignmentRepository assignmentRepository)
        {
            _assignmentRepository = assignmentRepository;
        }

        public async Task<List<Assignment>> GetActiveAssignmentsAsync()
        {
            return await _assignmentRepository.GetActiveAssignmentsAsync();
        }

        public async Task<Assignment?> GetAssignmentByIdAsync(int id)
        {
            return await _assignmentRepository.GetByIdAsync(id);
        }

        public async Task<Assignment?> GetAssignmentWithQuestionsAsync(int id)
        {
            return await _assignmentRepository.GetAssignmentWithQuestionsAsync(id);
        }

        public async Task<Assignment> CreateAssignmentAsync(Assignment assignment)
        {
            return await _assignmentRepository.CreateAssignmentAsync(assignment);
        }

        public async Task<Assignment> UpdateAssignmentAsync(Assignment assignment)
        {
            return await _assignmentRepository.UpdateAssignmentAsync(assignment);
        }

        public async Task<bool> DeleteAssignmentAsync(int id)
        {
            return await _assignmentRepository.DeleteAssignmentAsync(id);
        }
    }
}
