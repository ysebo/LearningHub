using LearningHub.Application.Interfaces.Services;
using LearningHub.Application.DTOs.Enrollments;
using Microsoft.AspNetCore.Mvc;

namespace LearningHub.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class EnrollmentsController(IEnrollmentService enrollmentService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<EnrollmentResponse>>> GetAll(CancellationToken cancellationToken)
    {
        var enrollments = await enrollmentService.GetAllAsync(cancellationToken);
        return Ok(enrollments);
    }

    [HttpPost]
    public async Task<ActionResult<EnrollmentResponse>> Create(
        [FromBody] CreateEnrollmentRequest request,
        CancellationToken cancellationToken)
    {
        var createdEnrollment = await enrollmentService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(
            nameof(GetAll),
            new { studentId = createdEnrollment.StudentId, courseId = createdEnrollment.CourseId },
            createdEnrollment);
    }

    [HttpPut("{studentId:int}/{courseId:int}")]
    public async Task<ActionResult<EnrollmentResponse>> Update(
        int studentId,
        int courseId,
        [FromBody] UpdateEnrollmentRequest request,
        CancellationToken cancellationToken)
    {
        var updatedEnrollment = await enrollmentService.UpdateAsync(
            studentId,
            courseId,
            request,
            cancellationToken);

        return Ok(updatedEnrollment);
    }

    [HttpDelete("{studentId:int}/{courseId:int}")]
    public async Task<IActionResult> Delete(
        int studentId,
        int courseId,
        CancellationToken cancellationToken)
    {
        await enrollmentService.DeleteAsync(studentId, courseId, cancellationToken);
        return NoContent();
    }
}
