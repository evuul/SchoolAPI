// IEnrollmentRepository.cs
using SchoolAPI.Models.DTOs.Enrollment;

namespace SchoolAPI.Repositories.Enrollment;

public interface IEnrollmentRepository
{
    Task<List<Models.Enrollment>> GetAllAsync(CancellationToken ct = default);
    Task<Models.Enrollment?> GetByIdAsync(int id, CancellationToken ct = default);
    Task AddAsync(Models.Enrollment enrollment, CancellationToken ct = default);
    Task DeleteAsync(Models.Enrollment enrollment, CancellationToken ct = default);

    // Behövs för att stoppa dubletter (StudentId + CourseId)
    Task<bool> EnrollmentExistsAsync(int studentId, int courseId, CancellationToken ct = default);

    Task<bool> SaveChangesAsync(CancellationToken ct = default);
}