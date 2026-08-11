using NoteProject.DTO.DiaryDTO;

namespace NoteProject.Interfaces
{
    public interface IDiaryService
    {
        Task<List<CreateDiary>> SearchDiaryByTime(DateTime? startTime, DateTime? endTime, Guid userId);
        Task<List<CreateDiary>> SearchDiaryByTitleAsync(string title, Guid userId);
    }
}
