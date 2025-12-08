namespace NoteProject.Models;

public class NoteTag
{
    public int NoteId { get; set; }
    public Notes Note { get; set; } = null!;

    public int TagId { get; set; }
    public Tag Tag { get; set; } = null!;
}
