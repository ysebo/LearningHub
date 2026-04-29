using LearningHub.Application.Interfaces.Persistence;
using LearningHub.Domain.Entities;
using LearningHub.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LearningHub.Infrastructure.Repositories;

public sealed class CategoryRepository(LearningHubDbContext dbContext): GenericRepository<Category>(dbContext), ICategoryRepository
{
    public override async Task<IReadOnlyList<Category>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await DbContext.Categories
            .Include(category => category.Courses)
            .AsNoTracking()
            .OrderBy(category => category.Name)
            .ToListAsync(cancellationToken);
    }

    public override async Task<Category?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await DbContext.Categories
            .Include(category => category.Courses)
            .AsNoTracking()
            .SingleOrDefaultAsync(category => category.Id == id, cancellationToken);
    }

    public async Task<bool> HasCoursesAsync(int categoryId, CancellationToken cancellationToken = default)
    {
        return await DbContext.Courses
            .AsNoTracking()
            .AnyAsync(course => course.CategoryId == categoryId, cancellationToken);
    }
}
