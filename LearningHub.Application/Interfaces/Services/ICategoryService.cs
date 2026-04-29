using LearningHub.Application.DTOs.Categories;
using System.Threading;
using System.Threading.Tasks;

namespace LearningHub.Application.Interfaces.Services;

public interface ICategoryService
{
    Task<IReadOnlyList<CategoryResponse>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<CategoryDetailsResponse> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<CategoryResponse> CreateAsync(CategoryRequest request, CancellationToken cancellationToken = default);

    Task<CategoryResponse> UpdateAsync(
        int id,
        CategoryRequest request,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}