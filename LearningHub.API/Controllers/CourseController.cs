using LearningHub.Application.Interfaces.Services;
using LearningHub.Application.DTOs.Courses;
using LearningHub.Application.DTOs.Enrollments;
using Microsoft.AspNetCore.Mvc;

namespace LearningHub.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class CoursesController(
    ICourseService courseService,
    IEnrollmentService enrollmentService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<CourseSummaryResponse>>> GetAll(
        [FromQuery] CourseQueryParameters queryParameters,
        CancellationToken cancellationToken)
    {
        var courses = await courseService.GetAllAsync(queryParameters, cancellationToken);
        return Ok(courses);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CourseDetailsResponse>> GetById(int id, CancellationToken cancellationToken)
    {
        var course = await courseService.GetByIdAsync(id, cancellationToken);
        return Ok(course);
    }

    [HttpGet("{id:int}/students")]
    public async Task<ActionResult<IReadOnlyList<CourseStudentResponse>>> GetStudents(
        int id,
        CancellationToken cancellationToken)
    {
        var students = await enrollmentService.GetStudentsForCourseAsync(id, cancellationToken);
        return Ok(students);
    }

    [HttpPost]
    public async Task<ActionResult<CourseSummaryResponse>> Create(
        [FromBody] CourseRequest request,
        CancellationToken cancellationToken)
    {
        var createdCourse = await courseService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = createdCourse.Id }, createdCourse);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<CourseSummaryResponse>> Update(
        int id,
        [FromBody] CourseRequest request,
        CancellationToken cancellationToken)
    {
        var updatedCourse = await courseService.UpdateAsync(id, request, cancellationToken);
        return Ok(updatedCourse);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await courseService.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}
