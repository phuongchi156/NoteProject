using NoteProject.DTO.NoteDTO;
using NoteProject.Models;

namespace NoteProject.Interfaces
{
    public interface INoteService
    {
        Task<List<GetNoteDTO>> SearchNoteAsync(string title, Guid userId);
        Task<List<GetNoteDTO>> SearchNoteByTime(DateTime startTime, DateTime endTime, Guid userId);
    }
}
