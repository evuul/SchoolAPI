namespace SchoolAPI.Models.DTOs;

public class StudentUpdateDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Email { get; set; } = string.Empty;
    public DateOnly? BirthDate { get; set; }
}