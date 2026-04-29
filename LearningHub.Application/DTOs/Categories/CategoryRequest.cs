using System.ComponentModel.DataAnnotations;

namespace LearningHub.Application.DTOs.Categories;

public sealed record CategoryRequest(
    string Name,

    string Description);
