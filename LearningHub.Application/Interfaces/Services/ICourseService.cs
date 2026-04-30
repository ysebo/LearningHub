using LearningHub.Application.DTOs.Courses;

namespace LearningHub.Application.Interfaces.Services;

public interface ICourseService
{
    Task<IReadOnlyList<CourseSummaryResponse>> GetAllAsync(
        CourseQueryParameters queryParameters,
        CancellationToken cancellationToken = default);

    Task<CourseDetailsResponse> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<CourseSummaryResponse> CreateAsync(
        CourseRequest request,
        CancellationToken cancellationToken = default);

    Task<CourseSummaryResponse> UpdateAsync(
        int id,
        CourseRequest request,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}
