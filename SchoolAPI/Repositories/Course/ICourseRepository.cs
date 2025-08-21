namespace SchoolAPI.Repositories.Course;

public interface ICourseRepository
{
    public Task<IEnumerable<Models.Course>> GetAllAsync(CancellationToken ct = default);
    public Task<Models.Course?> GetByIdAsync(int id, CancellationToken ct = default);
    public Task AddAsync(Models.Course course, CancellationToken ct = default);
    public Task UpdateAsync(Models.Course course, CancellationToken ct = default);
    public Task DeleteAsync(Models.Course course, CancellationToken ct = default);
    Task<bool> CourseExistsAsync(int courseId, CancellationToken ct = default);
    public Task<bool> SaveChangesAsync(CancellationToken ct = default);
}