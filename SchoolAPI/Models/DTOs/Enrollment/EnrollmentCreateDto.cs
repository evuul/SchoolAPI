namespace SchoolAPI.Models.DTOs.Enrollment;

public class EnrollmentCreateDto
{
    public int StudentId { get; set; }
    public int CourseId { get; set; }
    public string? Grade { get; set; }
}