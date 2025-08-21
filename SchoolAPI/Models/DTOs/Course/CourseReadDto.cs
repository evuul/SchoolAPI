namespace SchoolAPI.Models.DTOs.Course;

public class CourseReadDto
{
    public int Id { get; set; }
    public string CourseName { get; set; } = string.Empty;
    public int Credits { get; set; } 
}