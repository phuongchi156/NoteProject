namespace NoteProject.Models;

public class Notes
{
    public int Id { get; set; }
    public string Content { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; }
    public Guid UserId { get; set; }
    public string Title { get; set; } = null!;
    public string? Tags { get; set; }

    public User User { get; set; } = null!;
    public ICollection<NoteTag> NoteTags { get; set; } = new List<NoteTag>();
    public ICollection<ChecklistItems> ChecklistItems { get; set; } = new List<ChecklistItems>();

}
