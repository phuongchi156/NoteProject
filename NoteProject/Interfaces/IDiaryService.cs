namespace NoteProject.Interfaces
{
    public interface IDiaryService
    {
        Task SearchDiaryByTime(DateTime startTime, DateTime endTime, Guid userId);
        Task SearchDiaryByTitleAsync(string title, Guid userId);
    }
}
