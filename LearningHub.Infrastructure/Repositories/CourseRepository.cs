using LearningHub.Application.Interfaces.Persistence;
using LearningHub.Application.DTOs.Courses;
using LearningHub.Domain.Entities;
using LearningHub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LearningHub.Infrastructure.Repositories;

public sealed class CourseRepository(LearningHubDbContext dbContext)
    : GenericRepository<Course>(dbContext), ICourseRepository
{
    public override async Task<IReadOnlyList<Course>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await DbContext.Courses
            .Include(course => course.Category)
            .Include(course => course.Enrollments)
            .AsNoTracking()
            .OrderBy(course => course.Title)
            .ToListAsync(cancellationToken);
    }

    public override async Task<Course?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await DbContext.Courses
            .Include(course => course.Category)
            .Include(course => course.Enrollments)
            .AsNoTracking()
            .SingleOrDefaultAsync(course => course.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Course>> GetFilteredAsync(
        CourseQueryParameters queryParameters,
        CancellationToken cancellationToken = default)
    {
        IQueryable<Course> query = DbContext.Courses
            .Include(course => course.Category)
            .Include(course => course.Enrollments)
            .AsNoTracking();

        if (queryParameters.CategoryId.HasValue)
        {
            query = query.Where(course => course.CategoryId == queryParameters.CategoryId.Value);
        }

        if (!string.IsNullOrWhiteSpace(queryParameters.Search))
        {
            var search = queryParameters.Search.Trim().ToLowerInvariant();
            query = query.Where(course =>
                course.Title.ToLower().Contains(search) ||
                course.Description.ToLower().Contains(search));
        }

        if (queryParameters.MinPrice.HasValue)
        {
            query = query.Where(course => course.Price >= queryParameters.MinPrice.Value);
        }

        if (queryParameters.MaxPrice.HasValue)
        {
            query = query.Where(course => course.Price <= queryParameters.MaxPrice.Value);
        }

        var sortBy = queryParameters.SortBy?.Trim().ToLowerInvariant();
        var sortDirection = queryParameters.SortDirection?.Trim().ToLowerInvariant();
        var descending = sortDirection == "desc";

        query = (sortBy, descending) switch
        {
            ("price", true) => query.OrderByDescending(course => course.Price),
            ("price", false) => query.OrderBy(course => course.Price),
            ("title", true) => query.OrderByDescending(course => course.Title),
            _ => query.OrderBy(course => course.Title)
        };

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<Course?> GetDetailsByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await DbContext.Courses
            .Include(course => course.Category)
            .Include(course => course.Enrollments)
                .ThenInclude(enrollment => enrollment.Student)
            .AsNoTracking()
            .SingleOrDefaultAsync(course => course.Id == id, cancellationToken);
    }
}
