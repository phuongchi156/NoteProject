namespace NoteProject.Models
{
    public class DiaryImage
    {
        public Guid Id { get; set; }

        public string ImageUrl { get; set; }

        public Guid DiaryId { get; set; }

        public Diaries Diary { get; set; }
    }
}
