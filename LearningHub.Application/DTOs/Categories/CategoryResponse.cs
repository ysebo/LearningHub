namespace LearningHub.Application.DTOs.Categories;

public sealed record CategoryResponse(
    int Id,
    string Name,
    string Description,
    int CoursesCount);
