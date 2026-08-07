using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NoteProject.DTO.NoteDTO;
using NoteProject.Entities;
using NoteProject.Models;
using NoteProject.Services;
using System;

namespace NoteProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotesController : ControllerBase
    {
        private readonly NoteDbContext _context;
        //public NoteEntity NoteEntity { get; set; }
        public string UserId { get; set; }
        private readonly NoteService _noteService;
        public NotesController(NoteDbContext context, NoteService noteService)
        {
            _context = context;
            _noteService = noteService;
        }

        [HttpGet("Get all note")]
        public async Task<ActionResult<IEnumerable<Notes>>> GetNotes()
        {
            var user = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;
            var restult = _context.Notes.Where(n => n.UserId.ToString() == user).ToList();
            return restult;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Notes>> GetNote(int id)
        {
            var user = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;
            var note = await _context.Notes.FindAsync(id);

            if (note == null)
            {
                return NotFound();
            }

            return note;
        }

        [HttpPost]
        public async Task<ActionResult<Notes>> PostNote(CreateNote note)
        {

            UserId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;
            var newNote = new Notes
            {
                Title = note.Title,
                Content = note.Content,
                Tags = note.Tags,
                UserId = Guid.Parse(UserId)
            };

            _context.Notes.Add(newNote);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetNote), new { id = newNote.Id }, note);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PutNote(int id, CreateNote note)
        {
            var existingNote = await _context.Notes.FindAsync(id);

            if (existingNote == null) {
                return NotFound();
            }

            if (note.Title != null)
                existingNote.Title = note.Title;

            if (note.Content != null)
                existingNote.Content = note.Content;

            if (note.Tags != null)
                existingNote.Tags = note.Tags;

            _context.Notes.Update(existingNote);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNote(int id)
        {
            var user = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;
            var note = await _context.Notes.FindAsync(id);
            if (note == null)
            {
                return NotFound();
            }

            _context.Notes.Remove(note);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("search")]
        public async Task<List<GetNoteDTO>> SearchNoteAsync(string title)
        {
            var user = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;
            var userId = Guid.Parse(UserId);
            var result = await _noteService.SearchNoteAsync(title, userId);
            return result;
        }

        [HttpGet("searchByTime")]
        public async Task<List<GetNoteDTO>> SearchNoteByTime(DateTime startTime, DateTime endTime)
        {
            var user = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;
            var userId = Guid.Parse(UserId);
            var result = await _noteService.SearchNoteByTime(startTime, endTime, userId);
            return result;
        }
    }
}
