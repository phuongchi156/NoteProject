namespace NoteProject.Models;

public class ChecklistItems
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public bool IsCompleted { get; set; } = false;

    // Liên kết với Note
    public int NoteId { get; set; }
    public Notes Note { get; set; }
}
