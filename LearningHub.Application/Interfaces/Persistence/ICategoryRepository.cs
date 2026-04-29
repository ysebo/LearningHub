using LearningHub.Domain.Entities;

namespace LearningHub.Application.Interfaces.Persistence;

public interface ICategoryRepository : IGenericRepository<Category>
{
    Task<bool> HasCoursesAsync(int categoryId, CancellationToken cancellationToken = default);
}
