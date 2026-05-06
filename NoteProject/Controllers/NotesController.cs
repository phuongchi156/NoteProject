using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NoteProject.DTO.NoteDTO;
using NoteProject.Entities;
using NoteProject.Models;
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
        public NotesController(NoteDbContext context)
        {
            _context = context;
            //NoteEntity = noteEntity;
        }

        [HttpGet("Get all note")]
        public async Task<ActionResult<IEnumerable<Notes>>> GetNotes()
        {
            var user = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;
            var test = _context.Notes.Where(n => n.UserId.ToString() == user).ToList();
            return test;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Notes>> GetNote(int id)
        {
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

        [HttpPut("{id}")]
        public async Task<IActionResult> PutNote(int id, Notes note)
        {
            if (id != note.Id)
            {
                return BadRequest();
            }

            _context.Entry(note).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNote(int id)
        {
            var note = await _context.Notes.FindAsync(id);
            if (note == null)
            {
                return NotFound();
            }

            _context.Notes.Remove(note);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
