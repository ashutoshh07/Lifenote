using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Lifenote.Core.DTOs.Note;
using Lifenote.Core.Interfaces;

namespace Lifenote.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class NoteController : ControllerBase
    {
        private readonly INoteService _noteService;
        private readonly ICurrentUserService _currentUserService;

        public NoteController(INoteService noteService, ICurrentUserService currentUserService)
        {
            _noteService = noteService;
            _currentUserService = currentUserService;
        }

        // GET: api/notes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NoteDto>>> GetNotes()
        {
            var userId = await _currentUserService.GetCurrentUserIdAsync();
            var notes = await _noteService.GetAllNotesAsync(userId);
            return Ok(notes);
        }

        // GET: api/notes/pinned
        [HttpGet("pinned")]
        public async Task<ActionResult<IEnumerable<NoteDto>>> GetPinnedNotes()
        {
            var userId = await _currentUserService.GetCurrentUserIdAsync();
            var notes = await _noteService.GetPinnedNotesAsync(userId);
            return Ok(notes);
        }

        // GET: api/notes/search?q={searchTerm}
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<NoteDto>>> SearchNotes([FromQuery] string q)
        {
            var userId = await _currentUserService.GetCurrentUserIdAsync();
            var notes = await _noteService.SearchNotesAsync(userId, q);
            return Ok(notes);
        }

        // GET: api/notes/category/{category}
        [HttpGet("category/{category}")]
        public async Task<ActionResult<IEnumerable<NoteDto>>> GetNotesByCategory(string category)
        {
            var userId = await _currentUserService.GetCurrentUserIdAsync();
            var notes = await _noteService.GetNotesByCategoryAsync(userId, category);
            return Ok(notes);
        }

        // GET: api/notes/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<NoteDto>> GetNote(int id)
        {
            var userId = await _currentUserService.GetCurrentUserIdAsync();
            var note = await _noteService.GetNoteByIdAsync(id, userId);
            if (note == null) return NotFound();

            return Ok(note);
        }

        // POST: api/notes
        [HttpPost]
        public async Task<ActionResult<NoteDto>> CreateNote([FromBody] CreateNoteDto createNoteDto)
        {
            try
            {
                var userId = await _currentUserService.GetCurrentUserIdAsync();
                var created = await _noteService.CreateNoteAsync(userId, createNoteDto);
                return CreatedAtAction(nameof(GetNote), new { id = created.Id }, created);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/notes/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<NoteDto>> UpdateNote(int id, [FromBody] UpdateNoteDto updateNoteDto)
        {
            try
            {
                var userId = await _currentUserService.GetCurrentUserIdAsync();
                var updated = await _noteService.UpdateNoteAsync(id, userId, updateNoteDto);
                return Ok(updated);
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

        // DELETE: api/notes/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteNote(int id)
        {
            var userId = await _currentUserService.GetCurrentUserIdAsync();
            var result = await _noteService.DeleteNoteAsync(id, userId);
            if (!result) return NotFound();

            return NoContent();
        }

        // PATCH: api/notes/{id}/pin
        [HttpPatch("{id}/pin")]
        public async Task<ActionResult<NoteDto>> TogglePin(int id)
        {
            try
            {
                var userId = await _currentUserService.GetCurrentUserIdAsync();
                var note = await _noteService.TogglePinNoteAsync(id, userId);
                return Ok(note);
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
        }

        // PATCH: api/notes/{id}/archive
        [HttpPatch("{id}/archive")]
        public async Task<ActionResult<NoteDto>> ToggleArchive(int id)
        {
            try
            {
                var userId = await _currentUserService.GetCurrentUserIdAsync();
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
