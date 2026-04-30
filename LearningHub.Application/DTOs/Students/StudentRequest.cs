using System.ComponentModel.DataAnnotations;

namespace LearningHub.Application.DTOs.Students;

public sealed record StudentRequest(
    [Required(ErrorMessage = "Student name is required.")]
    [StringLength(120, MinimumLength = 3, ErrorMessage = "Student name must be between 3 and 120 characters.")]
    string FullName,

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Email must be valid.")]
    string Email,

    DateOnly DateOfBirth);
