using LearningHub.Application.DTOs.Courses;
using LearningHub.Domain.Entities;

namespace LearningHub.Application.Interfaces.Persistence;

public interface ICourseRepository : IGenericRepository<Course>
{
    Task<IReadOnlyList<Course>> GetFilteredAsync(
        CourseQueryParameters queryParameters,
        CancellationToken cancellationToken = default);

    Task<Course?> GetDetailsByIdAsync(int id, CancellationToken cancellationToken = default);
}
