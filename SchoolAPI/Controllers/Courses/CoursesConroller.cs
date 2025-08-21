using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using SchoolAPI.Models.DTOs.Course;
using SchoolAPI.Services.Course;

namespace SchoolAPI.Controllers.Courses;

[ApiController]
[Route("api/[controller]")]
public class CoursesConroller : ControllerBase
{
    private readonly ICourseService _service;

    public CoursesConroller(ICourseService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Models.DTOs.Course.CourseReadDto>>> GetAll(
        CancellationToken ct = default)
    {
        var courses = await _service.GetAllAsync(ct);
        return Ok(courses);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Models.DTOs.Course.CourseReadDto>> GetById(int id,
        CancellationToken ct = default)
    {
        var result = await _service.GetByIdAsync(id, ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<Models.DTOs.Course.CourseCreateDto>> Create(
        [FromBody] Models.DTOs.Course.CourseCreateDto dto, CancellationToken ct,
        IValidator<Models.DTOs.Course.CourseCreateDto> validator)
    {
        var result = validator.Validate(dto);
        if (!result.IsValid)
        {
            return BadRequest(result.Errors.Select(x => x.ErrorMessage));
        }

        var id = await _service.CreateAsync(dto, ct);
        var created = await _service.GetByIdAsync(id, ct);
        return CreatedAtAction(nameof(GetById), new { id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] CourseUpdateDto dto, CancellationToken ct, IValidator<CourseUpdateDto> validator )
    {
        var result = validator.Validate(dto);
        if (!result.IsValid)
            return BadRequest(result.Errors.Select(x => x.ErrorMessage));

        var isUpdated = await _service.UpdateAsync(id, dto, ct);
        return isUpdated ? NoContent() : NotFound();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var isDeleted = await _service.DeleteAsync(id, ct);
        return isDeleted ? NoContent() : NotFound();
    }
}