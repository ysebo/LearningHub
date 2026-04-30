using System.ComponentModel.DataAnnotations;

namespace LearningHub.Application.DTOs.Courses;

public sealed record CourseRequest(
    [Required(ErrorMessage = "Course title is required.")]
    [StringLength(120, MinimumLength = 3, ErrorMessage = "Course title must be between 3 and 120 characters.")]
    string Title,

    [StringLength(800, ErrorMessage = "Description can be at most 800 characters.")]
    string Description,

    [Range(0, 1000000, ErrorMessage = "Price cannot be negative.")]
    decimal Price,

    [Range(1, int.MaxValue, ErrorMessage = "CategoryId must be greater than 0.")]
    int CategoryId);
