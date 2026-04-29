namespace LearningHub.Domain.Entities;

public sealed class Category
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public ICollection<Course> Courses { get; set; } = new List<Course>();
}
