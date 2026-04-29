namespace LearningHub.Application.DTOs.Categories;

public sealed record CategoryDetailsResponse(
    int Id,
    string Name,
    string Description,
    IReadOnlyList<CategoryCourseResponse> Courses);

public sealed record CategoryCourseResponse(
    int Id,
    string Title,
    decimal Price);
