namespace NoteProject.Interfaces
{
    public interface INoteService
    {
        Task SearchNoteByTitleAsync(string title, Guid userId);
        Task SearchNoteByTime(DateTime startTime, DateTime endTime, Guid userId);
    }
}
