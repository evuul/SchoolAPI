using SchoolAPI.Models.DTOs.Enrollment;

namespace SchoolAPI.Repositories.Enrollment;

public interface IEnrollmentRepository
{
    Task<List<Models.Enrollment>> GetAllAsync(CancellationToken ct = default);
    Task<Models.Enrollment?> GetByIdAsync(int id, CancellationToken ct = default);
    Task AddAsync(Models.Enrollment enrollment, CancellationToken ct = default);
    Task DeleteAsync(Models.Enrollment enrollment, CancellationToken ct = default);
    Task<bool> SaveChangesAsync(CancellationToken ct = default);
}