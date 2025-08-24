using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using SchoolAPI.Models.DTOs.Enrollment;
using SchoolAPI.Services.Enrollment;

namespace SchoolAPI.Controllers.Enrollments;

[ApiController]
[Route("api/[controller]")]
public class EnrollmentsController : ControllerBase
{
    private readonly IEnrollmentService _enrollmentService;

    public EnrollmentsController(IEnrollmentService enrollmentService)
    {
        _enrollmentService = enrollmentService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<EnrollmentReadDto>>> GetAll(CancellationToken ct = default)
    {
        var items = await _enrollmentService.GetAllAsync(ct);
        return Ok(items);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct = default)
    {
        var result = await _enrollmentService.GetByIdAsync(id, ct);
        return this.ToActionResult(result); // 200/404/...
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] EnrollmentCreateDto dto,
        CancellationToken ct,
        IValidator<EnrollmentCreateDto> validator)
    {
        // Fältspecifik validering (behåll om du vill)
        var fv = await validator.ValidateAsync(dto, ct);
        if (!fv.IsValid)
        {
            foreach (var error in fv.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

            return ValidationProblem(ModelState);
        }

        var createResult = await _enrollmentService.CreateAsync(dto, ct);
        if (!createResult.IsSuccess)
            // Dubbelregistrering -> ServiceResult med ErrorKind.Validation -> 400 här
            return this.ToActionResult(createResult);

        // Hämta skapad resurs för payload + Location
        var readResult = await _enrollmentService.GetByIdAsync(createResult.Value, ct);
        if (readResult.IsSuccess)
            return CreatedAtAction(nameof(GetById), new { id = createResult.Value }, readResult.Value);

        // fallback: 201 utan body om läsningen misslyckades av någon anledning
        return StatusCode(201, new { id = createResult.Value });
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct = default)
    {
        var result = await _enrollmentService.DeleteAsync(id, ct);
        return this.ToActionResult(result, successStatus: 204); // 204 vid success, annars 404/...
    }
}