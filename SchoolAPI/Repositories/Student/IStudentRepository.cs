using SchoolAPI.Models;

namespace SchoolAPI.Repositories;

public interface IStudentRepository
{
    Task<List<Models.Student>> GetAllAsync(CancellationToken ct = default);
    Task<Models.Student?> GetByIdAsync(int id, CancellationToken ct = default);
    Task AddAsync(Models.Student student, CancellationToken ct = default);
    Task UpdateAsync(Models.Student student, CancellationToken ct = default);
    Task DeleteAsync(Models.Student student, CancellationToken ct = default);
    Task<bool> StudentExistsAsync(int studentId, CancellationToken ct = default);
    Task<bool> SaveChangesAsync(CancellationToken ct = default);
}