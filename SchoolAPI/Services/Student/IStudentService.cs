using SchoolAPI.Models.DTOs;
using SchoolAPI.Models.DTOs.Student;
namespace SchoolAPI.Services.Student;

public interface IStudentService
{
    Task<List<StudentReadDto>> GetAllAsync(CancellationToken ct = default);
    Task<StudentReadDto?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<int> CreateAsync(StudentCreateDto dto, CancellationToken ct = default);
    Task<bool> UpdateAsync(int id, StudentUpdateDto dto, CancellationToken ct = default);
    Task<bool> StudentExistsAsync(int studentId, CancellationToken ct = default);
    Task<bool> DeleteAsync(int id, CancellationToken ct = default);
}