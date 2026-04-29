using LearningHub.Application.Interfaces.Persistence;
using LearningHub.Application.DTOs.Categories;
using LearningHub.Domain.Entities;
using LearningHub.Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LearningHub.Application.Exceptions;
namespace LearningHub.Application.Services;

public sealed class CategoryService(ICategoryRepository categoryRepository) : ICategoryService
{
    public async Task<IReadOnlyList<CategoryResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var categories = await categoryRepository.GetAllAsync(cancellationToken);

        return categories
            .Select(category => new CategoryResponse(
                category.Id,
                category.Name,
                category.Description,
                category.Courses.Count))
            .ToList();
    }

    public async Task<CategoryDetailsResponse> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var category = await categoryRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException($"Category with id {id} was not found.");

        return new CategoryDetailsResponse(
            category.Id,
            category.Name,
            category.Description,
            category.Courses
                .OrderBy(course => course.Title)
                .Select(course => new CategoryCourseResponse(course.Id, course.Title, course.Price))
                .ToList());
    }

    public async Task<CategoryResponse> CreateAsync(
        CategoryRequest request,
        CancellationToken cancellationToken = default)
    {
        var category = new Category
        {
            Name = request.Name.Trim(),
            Description = request.Description.Trim()
        };

        var createdCategory = await categoryRepository.AddAsync(category, cancellationToken);

        return new CategoryResponse(
            createdCategory.Id,
            createdCategory.Name,
            createdCategory.Description,
            0);
    }

    public async Task<CategoryResponse> UpdateAsync(
        int id,
        CategoryRequest request,
        CancellationToken cancellationToken = default)
    {
        var category = await categoryRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException($"Category with id {id} was not found.");

        category.Name = request.Name.Trim();
        category.Description = request.Description.Trim();

        await categoryRepository.UpdateAsync(category, cancellationToken);

        return new CategoryResponse(
            category.Id,
            category.Name,
            category.Description,
            category.Courses.Count);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var category = await categoryRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException($"Category with id {id} was not found.");

        if (await categoryRepository.HasCoursesAsync(id, cancellationToken))
        {
            throw new BadRequestException(
                "You cannot delete a category while it still has courses inside it.");
        }

        await categoryRepository.DeleteAsync(category, cancellationToken);
    }
}