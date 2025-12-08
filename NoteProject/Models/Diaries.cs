namespace NoteProject.Models;

public class Diaries
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string Content { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; }
    public Guid UserId { get; set; }
    public bool IsPublic { get; set; }
    public User User { get; set; } = null!;
}
