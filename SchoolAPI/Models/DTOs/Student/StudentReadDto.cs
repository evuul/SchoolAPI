namespace SchoolAPI.Models.DTOs.Student;

public class StudentReadDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string? Email { get; set; } = string.Empty;
    public DateOnly? BirthDate { get; set; }
}