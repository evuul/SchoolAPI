using Microsoft.EntityFrameworkCore;
using SchoolAPI.Data;

namespace SchoolAPI.Repositories.Course;

public class CourseRepository : ICourseRepository
{
    private readonly SchoolContext _context;
    // Constructor that accepts a SchoolContext instance
    // and throws an ArgumentNullException if the context is null.
    
public CourseRepository(SchoolContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<IEnumerable<Models.Course>> GetAllAsync(CancellationToken ct = default)
    {
        return await _context.Courses
            .AsNoTracking()
            .OrderBy(c => c.Title)
            .ToListAsync(ct);
    }

    public async Task<Models.Course?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await _context.Courses.FirstOrDefaultAsync(c => c.Id == id, ct);
    }

    public async Task AddAsync(Models.Course course, CancellationToken ct = default)
    {
        await _context.Courses.AddAsync(course, ct);
    }

    public Task UpdateAsync(Models.Course course, CancellationToken ct = default)
    {
        _context.Courses.Update(course);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Models.Course course, CancellationToken ct = default)
    {
        _context.Courses.Remove(course);
        return Task.CompletedTask;
    }
    
public async Task<bool> CourseExistsAsync(int courseId, CancellationToken ct = default)
    {
        return await _context.Courses.AnyAsync(c => c.Id == courseId, ct);
    }

    public Task<bool> SaveChangesAsync(CancellationToken ct = default)
    {
        return _context.SaveChangesAsync(ct).ContinueWith(t => t.Result > 0, ct);
    }
}