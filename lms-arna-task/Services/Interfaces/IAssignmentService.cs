using lms_arna_task.Models;

namespace lms_arna_task.Services.Interfaces
{
    public interface IAssignmentService
    {
        Task<List<Assignment>> GetActiveAssignmentsAsync();
        Task<Assignment?> GetAssignmentByIdAsync(int id);
        Task<Assignment?> GetAssignmentWithQuestionsAsync(int id);
        Task<Assignment> CreateAssignmentAsync(Assignment assignment);
        Task<Assignment> UpdateAssignmentAsync(Assignment assignment);
        Task<bool> DeleteAssignmentAsync(int id);
    }
}
