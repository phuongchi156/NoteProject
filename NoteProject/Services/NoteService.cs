using Microsoft.EntityFrameworkCore;
using NoteProject.DTO.NoteDTO;
using NoteProject.Interfaces;
using NoteProject.Models;

namespace NoteProject.Services
{
    public class NoteService : INoteService
    {
        public NoteDbContext _context { get; set; }
        public NoteService(NoteDbContext context)
        {
            _context = context;
        }

        public async Task<List<GetNoteDTO>> SearchNoteAsync(string title, Guid userId)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new Exception("Title is empty.");
            }
            var notes = await _context.Notes
                .Where(n => n.UserId == userId && (n.Title.Contains(title) || n.Content.Contains(title)))
                .ToListAsync();

        var result = notes.Select(n => new GetNoteDTO
            {
                Title = n.Title,
                Content = n.Content,
                CreatedAt = n.CreatedAt,
                Tags = n.Tags,
            }).ToList();

            return result;
        }

        public async Task<List<GetNoteDTO>> SearchNoteByTime(DateTime? startDate, DateTime? endDate, Guid userId)
        {
            if (startDate > endDate)
            {
                throw new Exception("Start time must be less than or equal to end time.");
            }

            if (!startDate.HasValue && !endDate.HasValue)
            {
                throw new ArgumentException(
                    "At least one date must be provided.");
            }

            if (startDate.HasValue && !endDate.HasValue)
            {
                endDate = DateTime.Today.AddDays(1).AddTicks(-1);
            }

            if (!startDate.HasValue && endDate.HasValue)
            {
                startDate = DateTime.MinValue;
            }

            var notes = await _context.Notes
                .Where(n => n.UserId == userId && n.CreatedAt >= startDate && n.CreatedAt <= endDate)
                .ToListAsync();

            return notes.Select(n => new GetNoteDTO
            {
                Title = n.Title,
                Content = n.Content,
                CreatedAt = n.CreatedAt,
                Tags = n.Tags,
            }).ToList();
        }
    }
}
