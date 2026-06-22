using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoteProject.DTO.DiaryDTO;
using NoteProject.Models;

namespace NoteProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DiariesController : ControllerBase
    {
        private readonly NoteDbContext _context;
        public DiariesController(NoteDbContext context)
        {
            _context = context;
        }

        [HttpGet("Get all diaries")]
        public async Task<ActionResult<List<Diaries>>> GetAllDiaries()
        {
            var user = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;
            var restult = _context.Diaries.Where(n => n.UserId.ToString() == user).ToList();
            return restult;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Diaries>> GetDiary(Guid id)
        {
            var user = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;
            var diary = _context.Diaries.First(n => n.IsPublic || n.UserId.ToString() == user);

            if (diary == null)
            {
                return NotFound();
            }

            return diary;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDiary(Guid id)
        {
            var user = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;
            var diary = _context.Diaries.First(d => d.Id == id && d.UserId.ToString() == user);

            if (diary == null)
            {
                return NotFound();
            }
            _context.Diaries.Remove(diary);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDiary([FromBody] CreateDiary dto, Guid id)
        {
            var user = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;
            var diary = _context.Diaries.First(d => d.Id == id && d.UserId.ToString() == user);

            if (diary == null)
                return NotFound();

            diary.Title = dto.Title;
            diary.Content = dto.Content;
            diary.UpdatedAt = dto.DiaryDate;
            diary.IsPublic = dto.IsPublic;

            await _context.SaveChangesAsync();

            return Ok(diary);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDiary( [FromForm] CreateDiary dto)
        {
            var imageUrls = new List<DiaryImage>();

            foreach (var image in dto.Images)
            {
                var uploadsFolder = Path.Combine( Directory.GetCurrentDirectory(),
                    "wwwroot","images");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";

                var filePath = Path.Combine(uploadsFolder, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);

                await image.CopyToAsync(stream);
            }

            var user = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;

            var diary = new Diaries
            {
                Title = dto.Title,
                Content = dto.Content,
                UpdatedAt = dto.DiaryDate,
                IsPublic = dto.IsPublic,
                Images = imageUrls,
                UserId = Guid.Parse(user)
            };

            _context.Diaries.Add(diary);

            await _context.SaveChangesAsync();

            return Ok(diary);
        }
    }
    

}
