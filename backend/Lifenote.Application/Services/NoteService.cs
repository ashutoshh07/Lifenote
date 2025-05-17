using Lifenote.Application.DTOs;
using Lifenote.Core.Entities;
using Lifenote.Core.Interfaces;

namespace Lifenote.Application.Services;

public class NoteService
{
    private readonly IGenericRepository<note> _noteRepository;

    public NoteService(IGenericRepository<note> noteRepository)
    {
        _noteRepository = noteRepository;
    }

    public async Task<NoteDto> CreateNoteAsync(CreateNoteDto createNoteDto)
    {
        var Note = new note
        {
            noteid = Guid.NewGuid(),
            userid = createNoteDto.Userid,
            title = createNoteDto.Title,
            content = createNoteDto.Content,
            type = createNoteDto.Type,
            colortag = createNoteDto.Colortag,
            pinned = createNoteDto.Pinned,
            createdat = DateTime.UtcNow
        };

        await _noteRepository.AddAsync(Note);

        return MapToDto(Note);
    }

    public async Task<NoteDto?> GetNoteAsync(Guid id)
    {
        var note = await _noteRepository.GetByIdAsync(id);
        return note != null ? MapToDto(note) : null;
    }

    public async Task<IEnumerable<NoteDto>> GetUserNotesAsync(Guid userId)
    {
        var notes = await _noteRepository.FindAsync(n => n.userid == userId);
        return notes.Select(MapToDto);
    }

    public async Task UpdateNoteAsync(Guid id, CreateNoteDto updateNoteDto)
    {
        var note = await _noteRepository.GetByIdAsync(id);
        if (note == null) throw new Exception("Note not found");

        note.title = updateNoteDto.Title;
        note.content = updateNoteDto.Content;
        note.type = updateNoteDto.Type;
        note.colortag = updateNoteDto.Colortag;
        note.pinned = updateNoteDto.Pinned;
        note.updatedat = DateTime.UtcNow;

        await _noteRepository.UpdateAsync(note);
    }

    public async Task DeleteNoteAsync(Guid id)
    {
        var note = await _noteRepository.GetByIdAsync(id);
        if (note == null) throw new Exception("Note not found");

        await _noteRepository.DeleteAsync(note);
    }

    private static NoteDto MapToDto(note note)
    {
        return new NoteDto
        {
            Noteid = note.noteid,
            Userid = note.userid,
            Title = note.title,
            Content = note.content,
            Type = note.type,
            Colortag = note.colortag,
            Pinned = note.pinned,
            Createdat = note.createdat,
            Updatedat = note.updatedat
        };
    }
}
