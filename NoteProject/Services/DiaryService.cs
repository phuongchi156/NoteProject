using Microsoft.EntityFrameworkCore;
using NoteProject.DTO.DiaryDTO;

namespace NoteProject.Services
{
    public class DiaryService
    {
        private readonly NoteDbContext _context;
        public DiaryService(NoteDbContext context)
        {
            _context = context;
        }

        public async Task<List<CreateDiary>> SearchDiaryByTime(DateTime startTime, DateTime endTime, Guid userId)
        {
            if (startTime > endTime)
            {
                throw new Exception("Start time must be less than or equal to end time.");
            }
            var diaries = await _context.Diaries
                .Where(d => d.UserId == userId && d.CreatedAt >= startTime && d.CreatedAt <= endTime)
                .ToListAsync();


            var result = diaries.Select(d => new CreateDiary
            {
                Title = d.Title,
                Content = d.Content,
                DiaryDate = d.CreatedAt,
                IsPublic = d.IsPublic
            }).ToList();

            return result;
        }

        public async Task<List<CreateDiary>> SearchDiaryByTitleAsync(string title, Guid userId)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new Exception("Title is empty.");
            }

            var diaries = await _context.Diaries.Where(d => d.UserId == userId && (d.Title.Contains(title) || d.Content.Contains(title)))
                .ToListAsync();

            var result = diaries.Select(d => new CreateDiary
            {
                Title = d.Title,
                Content = d.Content,
                DiaryDate = d.CreatedAt,
                IsPublic = d.IsPublic
            }).ToList();

            return result;
        }
    }
}
