using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using SchoolAPI.Models.DTOs.Enrollment;
using SchoolAPI.Services.Enrollment;

namespace SchoolAPI.Controllers.Enrollments;

[Route("api/[controller]")]
    [ApiController]
    public class EnrollmentsController : ControllerBase
    {
        private readonly IEnrollmentService _enrollmentService;

        public EnrollmentsController(IEnrollmentService enrollmentService)
        {
            _enrollmentService = enrollmentService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EnrollmentReadDto>>> GetAll(CancellationToken ct)
        {
            return Ok(await _enrollmentService.GetAllAsync(ct));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<EnrollmentReadDto>> GetById(int id, CancellationToken ct = default)
        {
            var enrollment = await _enrollmentService.GetByIdAsync(id, ct);
            if (enrollment == null)
            {
                return NotFound();
            }

            return Ok(enrollment);
        }

        [HttpPost]
        public async Task<ActionResult<EnrollmentReadDto>> Create([FromBody] EnrollmentCreateDto dto,
            CancellationToken ct, IValidator<EnrollmentCreateDto> validator)
        {
            var validationResult = await validator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                return BadRequest(ModelState); // eller return ValidationProblem(ModelState); fÃ¶r mer info
            }

            var id = await _enrollmentService.CreateAsync(dto, ct);
            var createdEnrollment = await _enrollmentService.GetByIdAsync(id, ct);
            return CreatedAtAction(nameof(GetById), new { id }, createdEnrollment);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var isDeleted = await _enrollmentService.DeleteAsync(id, ct);
            return isDeleted ? NoContent() : NotFound();
        }
    }