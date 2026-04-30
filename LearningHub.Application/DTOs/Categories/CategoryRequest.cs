using System.ComponentModel.DataAnnotations;

namespace LearningHub.Application.DTOs.Categories;

public sealed record CategoryRequest(

    [Required(ErrorMessage = "Category name is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Category name must be between 2 and 100 characters.")]
    string Name,

    [property: StringLength(400, ErrorMessage = "Description can be at most 400 characters.")]
    string Description);
