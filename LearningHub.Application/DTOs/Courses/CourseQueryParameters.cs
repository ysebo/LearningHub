namespace LearningHub.Application.DTOs.Courses;

public sealed class CourseQueryParameters
{
    public int? CategoryId { get; init; }

    public string? Search { get; init; }

    public decimal? MinPrice { get; init; }

    public decimal? MaxPrice { get; init; }

    public string? SortBy { get; init; }

    public string? SortDirection { get; init; }
}
