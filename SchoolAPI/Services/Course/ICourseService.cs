using SchoolAPI.Models.DTOs.Course;

namespace SchoolAPI.Services.Course;

public interface ICourseService
{
    Task<IEnumerable<CourseReadDto>> GetAllAsync(CancellationToken ct = default);
    Task<CourseReadDto?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<int> CreateAsync(CourseCreateDto dto, CancellationToken ct = default);
    Task<bool> UpdateAsync(int id, CourseUpdateDto dto, CancellationToken ct = default);
    Task<bool> CourseExistsAsync(int courseId, CancellationToken ct = default);
    Task<bool> DeleteAsync(int id, CancellationToken ct = default);
}