// EnrollmentRepository.cs
using Microsoft.EntityFrameworkCore;
using SchoolAPI.Data;

namespace SchoolAPI.Repositories.Enrollment;

public class EnrollmentRepository : IEnrollmentRepository
{
    private readonly SchoolContext _context;

    public EnrollmentRepository(SchoolContext context) => _context = context;

    public async Task AddAsync(Models.Enrollment enrollment, CancellationToken ct = default)
    {
        await _context.Enrollments.AddAsync(enrollment, ct);
    }

    public async Task<List<Models.Enrollment>> GetAllAsync(CancellationToken ct = default)
    {
        return await _context.Enrollments
            .Include(e => e.Student)
            .Include(e => e.Course)
            .AsNoTracking()
            .OrderBy(e => e.Student.LastName)
            .ThenBy(e => e.Student.FirstName)
            .ToListAsync(ct);
    }

    public async Task<Models.Enrollment?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await _context.Enrollments
            .Include(e => e.Student)
            .Include(e => e.Course)
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id, ct);
    }

    public Task DeleteAsync(Models.Enrollment enrollment, CancellationToken ct = default)
    {
        _context.Enrollments.Remove(enrollment);
        return Task.CompletedTask;
    }

    public async Task<bool> EnrollmentExistsAsync(int studentId, int courseId, CancellationToken ct = default)
    {
        return await _context.Enrollments
            .AsNoTracking()
            .AnyAsync(e => e.StudentId == studentId && e.CourseId == courseId, ct);
    }

    public async Task<bool> SaveChangesAsync(CancellationToken ct = default)
    {
        var changed = await _context.SaveChangesAsync(ct);
        return changed > 0;
    }
}