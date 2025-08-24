using SchoolAPI.Models.DTOs.Course;

namespace SchoolAPI.Services.Course;

public interface ICourseService
{
    Task<IEnumerable<CourseReadDto>> GetAllAsync(CancellationToken ct = default);
    Task<ServiceResult<CourseReadDto>> GetByIdAsync(int id, CancellationToken ct = default);
    Task<ServiceResult<int>> CreateAsync(CourseCreateDto dto, CancellationToken ct = default);
    Task<ServiceResult<Unit>> UpdateAsync(int id, CourseUpdateDto dto, CancellationToken ct = default);
    Task<bool> CourseExistsAsync(int courseId, CancellationToken ct = default);
    Task<ServiceResult<Unit>> DeleteAsync(int id, CancellationToken ct = default);
}