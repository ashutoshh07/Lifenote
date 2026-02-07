using Microsoft.AspNetCore.Mvc;
using Lifenote.Core.Interfaces;
using Lifenote.Core.Models;
using Lifenote.Core.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace Lifenote.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class NoteController : ControllerBase
    {
        private readonly INoteService _noteService;

        public NoteController(INoteService noteService)
        {
            _noteService = noteService;
        }

        // GET: api/notes/{userId}
        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<Note>>> GetNotes(int userId)
        {
            var notes = await _noteService.GetAllNotesAsync(userId);
            return Ok(notes);
        }

        // GET: api/notes/{userId}/{id}
        [HttpGet("{userId}/{id}")]
        public async Task<ActionResult<Note>> GetNote(int userId, int id)
        {
            var note = await _noteService.GetNoteByIdAsync(id, userId);
            if (note == null) return NotFound();

            return Ok(note);
        }

        // POST: api/notes/{userId}
        [HttpPost("{userId}")]
        public async Task<ActionResult<Note>> CreateNote(int userId, CreateNoteDto createNoteDto)
        {
            var note = new Note
            {
                UserId = userId,
                Title = createNoteDto.Title,
                Content = createNoteDto.Content,
                Category = createNoteDto.Category,
                Tags = createNoteDto.Tags,
                IsPinned = createNoteDto.IsPinned
            };

            try
            {
                var createdNote = await _noteService.CreateNoteAsync(note);
                return CreatedAtAction(nameof(GetNote), new { userId, id = createdNote.Id }, createdNote);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/notes/{userId}/{id}
        [HttpPut("{userId}/{id}")]
        public async Task<ActionResult<Note>> UpdateNote(int userId, int id, UpdateNoteDto updateNoteDto)
        {
            var note = new Note
            {
                Id = id,
                UserId = userId,
                Title = updateNoteDto.Title,
                Content = updateNoteDto.Content,
                Category = updateNoteDto.Category,
                Tags = updateNoteDto.Tags,
                IsPinned = updateNoteDto.IsPinned,
                IsArchived = updateNoteDto.IsArchived
            };

            try
            {
                var updatedNote = await _noteService.UpdateNoteAsync(note);
                return Ok(updatedNote);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UnauthorizedAccessException)
            {
                return NotFound();
            }
        }

        // DELETE: api/notes/{userId}/{id}
        [HttpDelete("{userId}/{id}")]
        public async Task<ActionResult> DeleteNote(int userId, int id)
        {
            var result = await _noteService.DeleteNoteAsync(id, userId);
            if (!result) return NotFound();

            return NoContent();
        }

        // GET: api/notes/{userId}/category/{category}
        [HttpGet("{userId}/category/{category}")]
        public async Task<ActionResult<IEnumerable<Note>>> GetNotesByCategory(int userId, string category)
        {
            var notes = await _noteService.GetNotesByCategoryAsync(userId, category);
            return Ok(notes);
        }

        // GET: api/notes/{userId}/pinned
        [HttpGet("{userId}/pinned")]
        public async Task<ActionResult<IEnumerable<Note>>> GetPinnedNotes(int userId)
        {
            var notes = await _noteService.GetPinnedNotesAsync(userId);
            return Ok(notes);
        }

        // GET: api/notes/{userId}/search?q={searchTerm}
        [HttpGet("{userId}/search")]
        public async Task<ActionResult<IEnumerable<Note>>> SearchNotes(int userId, [FromQuery] string q)
        {
            var notes = await _noteService.SearchNotesAsync(userId, q);
            return Ok(notes);
        }

        // PATCH: api/notes/{userId}/{id}/pin
        [HttpPatch("{userId}/{id}/pin")]
        public async Task<ActionResult<Note>> TogglePin(int userId, int id)
        {
            try
            {
                var note = await _noteService.TogglePinNoteAsync(id, userId);
                return Ok(note);
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
        }

        // PATCH: api/notes/{userId}/{id}/archive
        [HttpPatch("{userId}/{id}/archive")]
        public async Task<ActionResult<Note>> ToggleArchive(int userId, int id)
        {
            try
            {
                var note = await _noteService.ToggleArchiveNoteAsync(id, userId);
                return Ok(note);
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
        }
    }
}
