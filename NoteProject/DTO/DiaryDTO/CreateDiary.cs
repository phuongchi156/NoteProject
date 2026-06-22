namespace NoteProject.DTO.DiaryDTO
{
    public class CreateDiary
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime DiaryDate { get; set; }
        public bool IsPublic { get; set; } = false;

        public List<IFormFile> Images { get; set; } = new();
    }
}
