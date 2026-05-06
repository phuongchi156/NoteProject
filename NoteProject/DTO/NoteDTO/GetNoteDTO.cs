namespace NoteProject.DTO.NoteDTO
{
    public class GetNoteDTO
    {
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string? Tags { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
