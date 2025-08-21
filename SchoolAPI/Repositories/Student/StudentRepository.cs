using Microsoft.EntityFrameworkCore;
using SchoolAPI.Data;

namespace SchoolAPI.Repositories.Student;

public class StudentRepository : IStudentRepository
{
    private readonly SchoolContext _context;
    // constructor that accepts a SchoolContext instance
    // and throws an ArgumentNullException if the context is null.
    public StudentRepository(SchoolContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    
    public async Task<List<Models.Student>> GetAllAsync(CancellationToken ct = default)
    {
        return await _context.Students
            .AsNoTracking()
            .OrderBy(s => s.LastName)
            .ThenBy(s => s.FirstName)
            .ToListAsync(ct);
    }

    public async Task<Models.Student?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await _context.Students.FirstOrDefaultAsync(s => s.Id == id, ct);
    }

    public async Task AddAsync(Models.Student student, CancellationToken ct = default)
    {
        await _context.Students.AddAsync(student, ct);
    }

    public Task UpdateAsync(Models.Student student, CancellationToken ct = default)
    {
        _context.Students.Update(student);
        return Task.CompletedTask;
    }
    
    public async Task<bool> StudentExistsAsync(int studentId, CancellationToken ct = default)
    {
        return await _context.Students.AnyAsync(s => s.Id == studentId, ct);
    }

    public Task DeleteAsync(Models.Student student, CancellationToken ct = default)
    {
        _context.Students.Remove(student);
        return Task.CompletedTask;
    }

    public Task<bool> SaveChangesAsync(CancellationToken ct = default)
    {
        return _context.SaveChangesAsync(ct).ContinueWith(t => t.Result > 0, ct);
    }
}