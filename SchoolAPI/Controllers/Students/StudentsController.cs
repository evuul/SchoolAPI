using FluentValidation;
using Microsoft.AspNetCore.Mvc;
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
    public async Task<IActionResult> GetById(int id, CancellationToken ct = default)
    {
        var result = await _service.GetByIdAsync(id, ct);
        return this.ToActionResult(result); // 200 eller 404 via ResultToHttp
    }

    [HttpPost]
    [ProducesResponseType(typeof(StudentReadDto), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Create(
        [FromBody] StudentCreateDto dto,
        CancellationToken ct,
        IValidator<StudentCreateDto> validator)
    {
        // (valfritt) fältspecifik validering
        var fv = await validator.ValidateAsync(dto, ct);
        if (!fv.IsValid)
            return BadRequest(new { errors = fv.Errors.Select(e => e.ErrorMessage) });

        var createResult = await _service.CreateAsync(dto, ct);
        if (!createResult.IsSuccess)
            return this.ToActionResult(createResult); // 400/… via helpern

        // Hämta för payload + Location
        var getResult = await _service.GetByIdAsync(createResult.Value, ct);
        if (getResult.IsSuccess)
            return CreatedAtAction(nameof(GetById), new { id = createResult.Value }, getResult.Value);

        // Fallback: 201 med id om läsningen skulle fallera
        return StatusCode(201, new { id = createResult.Value });
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] StudentUpdateDto dto,
        CancellationToken ct,
        IValidator<StudentUpdateDto> validator)
    {
        var fv = await validator.ValidateAsync(dto, ct);
        if (!fv.IsValid)
            return BadRequest(new { errors = fv.Errors.Select(e => e.ErrorMessage) });

        var result = await _service.UpdateAsync(id, dto, ct);
        return this.ToActionResult(result, successStatus: 204); // 204 vid success
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    [ProducesResponseType(403)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var result = await _service.DeleteAsync(id, ct);
        return this.ToActionResult(result, successStatus: 204); // 403 om aktiva inskrivningar, 404 om saknas
    }
}