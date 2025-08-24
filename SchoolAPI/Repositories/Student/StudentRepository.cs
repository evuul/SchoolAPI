using Microsoft.EntityFrameworkCore;
using SchoolAPI.Data;

namespace SchoolAPI.Repositories.Student;

public class StudentRepository : IStudentRepository
{
    private readonly SchoolContext _context;

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
        // Behåll tracking här så Update/Delete kan använda entiteten
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
        return await _context.Students.AsNoTracking().AnyAsync(s => s.Id == studentId, ct);
    }

    public Task DeleteAsync(Models.Student student, CancellationToken ct = default)
    {
        _context.Students.Remove(student);
        return Task.CompletedTask;
    }

    // 🔹 Nya för affärsreglerna
    public async Task<bool> EmailExistsAsync(string email, int? exceptId, CancellationToken ct = default)
    {
        var q = _context.Students.AsNoTracking().Where(s => s.Email == email);
        if (exceptId.HasValue) q = q.Where(s => s.Id != exceptId.Value);
        return await q.AnyAsync(ct);
    }

    public async Task<bool> HasActiveEnrollmentsAsync(int studentId, CancellationToken ct = default)
    {
        // Justera villkoret om ni har ett "IsActive"-fält; annars räcker StudentId
        return await _context.Enrollments
            .AsNoTracking()
            .AnyAsync(e => e.StudentId == studentId, ct);
    }

    public async Task<bool> SaveChangesAsync(CancellationToken ct = default)
    {
        var changed = await _context.SaveChangesAsync(ct);
        return changed > 0;
    }
}