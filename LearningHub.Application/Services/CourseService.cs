using LearningHub.Application.Interfaces.Persistence;
using LearningHub.Application.Interfaces.Services;
using LearningHub.Application.DTOs.Courses;
using LearningHub.Application.Exceptions;
using LearningHub.Domain.Entities;

namespace LearningHub.Application.Services;

public sealed class CourseService(ICourseRepository courseRepository, ICategoryRepository categoryRepository) : ICourseService
{
    public async Task<IReadOnlyList<CourseSummaryResponse>> GetAllAsync(
        CourseQueryParameters queryParameters,
        CancellationToken cancellationToken = default)
    {
        var courses = await courseRepository.GetFilteredAsync(queryParameters, cancellationToken);

        return courses.Select(MapCourseSummary).ToList();
    }

    public async Task<CourseDetailsResponse> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var course = await courseRepository.GetDetailsByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException($"Course with id {id} was not found.");

        return new CourseDetailsResponse(
            course.Id,
            course.Title,
            course.Description,
            course.Price,
            course.CategoryId,
            course.Category?.Name ?? string.Empty,
            course.Enrollments
                .OrderBy(enrollment => enrollment.Student!.FullName)
                .Select(enrollment => new CourseStudentDetailsResponse(
                    enrollment.StudentId,
                    enrollment.Student!.FullName,
                    enrollment.Student.Email,
                    enrollment.EnrolledAt,
                    enrollment.Status))
                .ToList());
    }

    public async Task<CourseSummaryResponse> CreateAsync(
        CourseRequest request,
        CancellationToken cancellationToken = default)
    {
        await EnsureCategoryExistsAsync(request.CategoryId, cancellationToken);

        var course = new Course
        {
            Title = request.Title.Trim(),
            Description = request.Description.Trim(),
            Price = request.Price,
            CategoryId = request.CategoryId
        };

        var createdCourse = await courseRepository.AddAsync(course, cancellationToken);
        var courseWithCategory = await courseRepository.GetByIdAsync(createdCourse.Id, cancellationToken)
            ?? throw new NotFoundException($"Course with id {createdCourse.Id} was not found after creation.");

        return MapCourseSummary(courseWithCategory);
    }

    public async Task<CourseSummaryResponse> UpdateAsync(
        int id,
        CourseRequest request,
        CancellationToken cancellationToken = default)
    {
        await EnsureCategoryExistsAsync(request.CategoryId, cancellationToken);

        var course = await courseRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException($"Course with id {id} was not found.");

        course.Title = request.Title.Trim();
        course.Description = request.Description.Trim();
        course.Price = request.Price;
        course.CategoryId = request.CategoryId;

        await courseRepository.UpdateAsync(course, cancellationToken);

        var updatedCourse = await courseRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException($"Course with id {id} was not found after update.");

        return MapCourseSummary(updatedCourse);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var course = await courseRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException($"Course with id {id} was not found.");

        await courseRepository.DeleteAsync(course, cancellationToken);
    }

    private async Task EnsureCategoryExistsAsync(int categoryId, CancellationToken cancellationToken)
    {
        if (!await categoryRepository.ExistsAsync(categoryId, cancellationToken))
        {
            throw new NotFoundException($"Category with id {categoryId} was not found.");
        }
    }

    private static CourseSummaryResponse MapCourseSummary(Course course)
    {
        return new CourseSummaryResponse(
            course.Id,
            course.Title,
            course.Description,
            course.Price,
            course.CategoryId,
            course.Category?.Name ?? string.Empty,
            course.Enrollments.Count);
    }
}
