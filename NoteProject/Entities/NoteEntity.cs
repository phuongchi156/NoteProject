using NoteProject.Models;

namespace NoteProject.Entities;

public class NoteEntity
{
    public string Content { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; }
    public string Title { get; set; } = null!;
    public string? Tags { get; set; }
    public ICollection<NoteTag> NoteTags { get; set; } = new List<NoteTag>();
}