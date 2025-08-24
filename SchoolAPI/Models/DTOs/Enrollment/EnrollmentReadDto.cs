namespace SchoolAPI.Models.DTOs.Enrollment;

public class EnrollmentReadDto
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public int CourseId { get; set; }
    public DateTime EnrollmentDate { get; set; } // UTC
    public string CourseTitle { get; set; } = string.Empty;
    public string StudentFullName { get; set; } = string.Empty;
    public string? Grade { get; set; }
}