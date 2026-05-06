namespace NoteProject.DTO.NoteDTO
{
    public class CreateNote
    {
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string? Tags { get; set; }
    }
}
