using Azure;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using SchoolAPI.Models;
using SchoolAPI.Models.DTOs;
using SchoolAPI.Models.DTOs.Student;
using SchoolAPI.Services.Student;

namespace SchoolAPI.Controllers.Students;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly IStudentService _service;

    public StudentsController(IStudentService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<StudentReadDto>>> GetAll(CancellationToken ct = default)
    {
        var students = await _service.GetAllAsync(ct);
        return Ok(students);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<StudentReadDto>> GetById(int id, CancellationToken ct = default)
    {
        var result = await _service.GetByIdAsync(id, ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(StudentCreateDto), 201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<StudentCreateDto>> Create([FromBody] StudentCreateDto dto,
        CancellationToken ct, IValidator<StudentCreateDto> validator)
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
    public async Task<IActionResult> Update(int id, [FromBody] StudentUpdateDto dto,
        CancellationToken ct, IValidator<StudentUpdateDto> validator)
    {
        var result = validator.Validate(dto);
        if (!result.IsValid)
        {
            return BadRequest(result.Errors.Select(x => x.ErrorMessage));
        }
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