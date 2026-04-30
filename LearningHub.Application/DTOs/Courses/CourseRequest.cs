using System.ComponentModel.DataAnnotations;

namespace LearningHub.Application.DTOs.Courses;

public sealed record CourseRequest(
    [property: Required(ErrorMessage = "Course title is required.")]
    [property: StringLength(120, MinimumLength = 3, ErrorMessage = "Course title must be between 3 and 120 characters.")]
    string Title,

    [property: StringLength(800, ErrorMessage = "Description can be at most 800 characters.")]
    string Description,

    [property: Range(0, 1000000, ErrorMessage = "Price cannot be negative.")]
    decimal Price,

    [property: Range(1, int.MaxValue, ErrorMessage = "CategoryId must be greater than 0.")]
    int CategoryId);
