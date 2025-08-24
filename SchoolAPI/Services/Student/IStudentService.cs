using SchoolAPI.Models.DTOs;
using SchoolAPI.Models.DTOs.Student;

namespace SchoolAPI.Services.Student;

public interface IStudentService
{
    Task<List<StudentReadDto>> GetAllAsync(CancellationToken ct = default);
    Task<ServiceResult<StudentReadDto>> GetByIdAsync(int id, CancellationToken ct = default);
    Task<ServiceResult<int>> CreateAsync(StudentCreateDto dto, CancellationToken ct = default);
    Task<ServiceResult<Unit>> UpdateAsync(int id, StudentUpdateDto dto, CancellationToken ct = default);
    Task<bool> StudentExistsAsync(int studentId, CancellationToken ct = default);
    Task<ServiceResult<Unit>> DeleteAsync(int id, CancellationToken ct = default);
}