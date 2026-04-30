namespace LearningHub.Application.DTOs.Courses;

public sealed record CourseSummaryResponse(
    int Id,
    string Title,
    string Description,
    decimal Price,
    int CategoryId,
    string CategoryName,
    int StudentsCount);
