using lms_arna_task.Data;
using lms_arna_task.Models;
using lms_arna_task.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace lms_arna_task.Repositories
{
    public class AssignmentRepository : IAssignmentRepository
    {
        private readonly ApplicationDbContext _context;

        public AssignmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Assignment>> GetAllAsync()
        {
            return await _context.Assignments.ToListAsync();
        }

        public async Task<Assignment?> GetByIdAsync(int id)
        {
            return await _context.Assignments
                .Include(a => a.CreatedBy)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<List<Assignment>> GetActiveAssignmentsAsync()
        {
            return await _context.Assignments
                .Where(a => a.IsActive && a.StartDate <= DateTime.UtcNow && a.EndDate >= DateTime.UtcNow)
                .Include(a => a.CreatedBy)
                .OrderBy(a => a.StartDate)
                .ToListAsync();
        }

        public async Task<Assignment?> GetAssignmentWithQuestionsAsync(int id)
        {
            return await _context.Assignments
                .Include(a => a.Questions)
                .Include(a => a.CreatedBy)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Assignment> CreateAssignmentAsync(Assignment assignment)
        {
            assignment.CreatedAt = DateTime.UtcNow;
            assignment.UpdatedAt = DateTime.UtcNow;

            _context.Assignments.Add(assignment);
            await _context.SaveChangesAsync();
            return assignment;
        }

        public async Task<Assignment> UpdateAssignmentAsync(Assignment assignment)
        {
            assignment.UpdatedAt = DateTime.UtcNow;
            _context.Assignments.Update(assignment);
            await _context.SaveChangesAsync();
            return assignment;
        }

        public async Task<bool> DeleteAssignmentAsync(int id)
        {
            var assignment = await _context.Assignments.FindAsync(id);
            if (assignment == null) return false;

            _context.Assignments.Remove(assignment);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
