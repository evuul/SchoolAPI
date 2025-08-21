using Microsoft.EntityFrameworkCore;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using SchoolAPI.Data;
using SchoolAPI.Models.DTOs.Enrollment;

namespace SchoolAPI.Repositories.Enrollment;

public class EnrollmentRepository : IEnrollmentRepository
{
    private readonly SchoolContext _context;

    public EnrollmentRepository(SchoolContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Models.Enrollment enrollment, CancellationToken ct = default)
    {
        await _context.Enrollments.AddAsync(enrollment);
    }

    public Task DeleteAsync(Models.Course course, CancellationToken ct = default)
    {
        throw new NotImplementedException();
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

    public Task<bool> SaveChangesAsync(CancellationToken ct = default)
    {
        return _context.SaveChangesAsync(ct).ContinueWith(t => t.Result > 0, ct);
    }

    public Task DeleteAsync(Models.Enrollment enrollment, CancellationToken ct = default)
    {
        _context.Enrollments.Remove(enrollment);
        return Task.CompletedTask;
    }
}