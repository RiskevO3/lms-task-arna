using lms_arna_task.Models;

namespace lms_arna_task.Repositories.Interfaces
{
    public interface IAssignmentRepository
    {
        Task<IEnumerable<Assignment>> GetAllAsync();
        Task<Assignment?> GetByIdAsync(int id);
        Task<List<Assignment>> GetActiveAssignmentsAsync();
        Task<Assignment?> GetAssignmentWithQuestionsAsync(int id);
        Task<Assignment> CreateAssignmentAsync(Assignment assignment);
        Task<Assignment> UpdateAssignmentAsync(Assignment assignment);
        Task<bool> DeleteAssignmentAsync(int id);
    }
}
