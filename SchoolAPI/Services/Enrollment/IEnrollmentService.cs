using SchoolAPI.Models.DTOs.Enrollment;

namespace SchoolAPI.Services.Enrollment;

public interface IEnrollmentService
{
    Task<List<EnrollmentReadDto>> GetAllAsync(CancellationToken ct = default);
    Task<EnrollmentReadDto?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<int> CreateAsync(EnrollmentCreateDto dto, CancellationToken ct = default);
    Task<bool> DeleteAsync(int id, CancellationToken ct = default);
}