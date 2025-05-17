using Lifenote.Application.DTOs;
using Lifenote.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lifenote.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotesController : ControllerBase
{
    private readonly NoteService _noteService;

    public NotesController(NoteService noteService)
    {
        _noteService = noteService;
    }

    [HttpPost]
    public async Task<ActionResult<NoteDto>> CreateNote(CreateNoteDto createNoteDto)
    {
        var note = await _noteService.CreateNoteAsync(createNoteDto);
        return CreatedAtAction(nameof(GetNote), new { id = note.Noteid }, note);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<NoteDto>> GetNote(Guid id)
    {
        var note = await _noteService.GetNoteAsync(id);
        if (note == null) return NotFound();
        return Ok(note);
    }

    [HttpGet("user/{userId:guid}")]
    public async Task<ActionResult<IEnumerable<NoteDto>>> GetUserNotes(Guid userId)
    {
        var notes = await _noteService.GetUserNotesAsync(userId);
        return Ok(notes);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateNote(Guid id, CreateNoteDto updateNoteDto)
    {
        try
        {
            await _noteService.UpdateNoteAsync(id, updateNoteDto);
            return NoContent();
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteNote(Guid id)
    {
        try
        {
            await _noteService.DeleteNoteAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
}
