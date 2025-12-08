using Microsoft.AspNetCore.Http.HttpResults;

namespace NoteProject.Models;

public class Tag
{
    public int TagId { get; set; }
    public string Name { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public ICollection<NoteTag> NoteTags { get; set; } = new List<NoteTag>();

}
