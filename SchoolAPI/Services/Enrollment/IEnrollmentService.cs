// Services/Enrollment/IEnrollmentService.cs
using SchoolAPI.Models.DTOs.Enrollment;

namespace SchoolAPI.Services.Enrollment;

public interface IEnrollmentService
{
    Task<List<EnrollmentReadDto>> GetAllAsync(CancellationToken ct = default);                 // ok att lämna "ren" lista
    Task<ServiceResult<EnrollmentReadDto>> GetByIdAsync(int id, CancellationToken ct = default);
    Task<ServiceResult<int>> CreateAsync(EnrollmentCreateDto dto, CancellationToken ct = default);
    Task<ServiceResult<Unit>> DeleteAsync(int id, CancellationToken ct = default);
}