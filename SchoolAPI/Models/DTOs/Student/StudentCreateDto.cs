namespace SchoolAPI.Models.DTOs;

public class StudentCreateDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Email { get; set; } = string.Empty;
    public DateOnly? BirthDate { get; set; }
}