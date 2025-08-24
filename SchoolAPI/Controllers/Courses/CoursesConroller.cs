using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using SchoolAPI.Models.DTOs.Course;
using SchoolAPI.Services.Course;

namespace SchoolAPI.Controllers.Courses;

[ApiController]
[Route("api/[controller]")]
public class CoursesController : ControllerBase
{
    private readonly ICourseService _service;

    public CoursesController(ICourseService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CourseReadDto>>> GetAll(CancellationToken ct = default)
    {
        var courses = await _service.GetAllAsync(ct);
        return Ok(courses);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct = default)
    {
        var result = await _service.GetByIdAsync(id, ct);
        return this.ToActionResult(result); // använder ResultToHttp.ToActionResult
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CourseCreateDto dto,
        CancellationToken ct,
        IValidator<CourseCreateDto> validator)
    {
        // (Fältspecifik) FluentValidation – valfritt att behålla
        var fv = validator.Validate(dto);
        if (!fv.IsValid)
            return BadRequest(new { errors = fv.Errors.Select(e => e.ErrorMessage) });

        var createResult = await _service.CreateAsync(dto, ct);
        if (createResult.IsSuccess)
        {
            // Hämta den skapade för payload/Location
            var getResult = await _service.GetByIdAsync(createResult.Value!, ct);
            if (getResult.IsSuccess)
                return CreatedAtAction(nameof(GetById), new { id = createResult.Value }, getResult.Value);
            // Skapad men kunde inte läsa – fall tillbaka till 201 utan body
            return StatusCode(201, new { id = createResult.Value });
        }

        // Kredits < 0 etc → 400 via helpern
        return this.ToActionResult(createResult); // default 200 → vi vill 4xx/5xx för fel
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] CourseUpdateDto dto,
        CancellationToken ct,
        IValidator<CourseUpdateDto> validator)
    {
        // (Fältspecifik) FluentValidation – valfritt
        var fv = validator.Validate(dto);
        if (!fv.IsValid)
            return BadRequest(new { errors = fv.Errors.Select(e => e.ErrorMessage) });

        var result = await _service.UpdateAsync(id, dto, ct);
        // Vid success: 204 No Content
        return this.ToActionResult(result, successStatus: 204);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var result = await _service.DeleteAsync(id, ct);
        // Vid success: 204 No Content
        return this.ToActionResult(result, successStatus: 204);
    }
}