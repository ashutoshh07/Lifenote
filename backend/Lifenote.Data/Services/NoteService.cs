using Lifenote.Core.Interfaces;
using Lifenote.Core.Models;

namespace Lifenote.Data.Services
{
    public class NoteService : INoteService
    {
        private readonly INoteRepository _noteRepository;

        public NoteService(INoteRepository noteRepository)
        {
            _noteRepository = noteRepository;
        }

        public async Task<IEnumerable<Note>> GetAllNotesAsync(int userId)
        {
            return await _noteRepository.GetAllAsync(userId);
        }

        public async Task<Note?> GetNoteByIdAsync(int id, int userId)
        {
            var note = await _noteRepository.GetByIdAsync(id);
            if (note == null || note.UserId != userId) return null;

            return note;
        }

        public async Task<Note> CreateNoteAsync(Note note)
        {
            if (string.IsNullOrWhiteSpace(note.Title))
                throw new ArgumentException("Note title cannot be empty");

            if (string.IsNullOrWhiteSpace(note.Content))
                throw new ArgumentException("Note content cannot be empty");

            return await _noteRepository.CreateAsync(note);
        }

        public async Task<Note> UpdateNoteAsync(Note note)
        {
            if (string.IsNullOrWhiteSpace(note.Title))
                throw new ArgumentException("Note title cannot be empty");

            if (string.IsNullOrWhiteSpace(note.Content))
                throw new ArgumentException("Note content cannot be empty");

            var existingNote = await _noteRepository.GetByIdAsync(note.Id);
            if (existingNote == null || existingNote.UserId != note.UserId)
                throw new UnauthorizedAccessException("Note not found or access denied");

            return await _noteRepository.UpdateAsync(note);
        }

        public async Task<bool> DeleteNoteAsync(int id, int userId)
        {
            if (!await _noteRepository.ExistsAsync(id, userId))
                return false;

            return await _noteRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Note>> GetNotesByCategoryAsync(int userId, string category)
        {
            return await _noteRepository.GetByCategoryAsync(userId, category);
        }

        public async Task<IEnumerable<Note>> GetPinnedNotesAsync(int userId)
        {
            return await _noteRepository.GetPinnedAsync(userId);
        }

        public async Task<IEnumerable<Note>> SearchNotesAsync(int userId, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetAllNotesAsync(userId);

            return await _noteRepository.SearchAsync(userId, searchTerm);
        }

        public async Task<Note> TogglePinNoteAsync(int id, int userId)
        {
            var note = await GetNoteByIdAsync(id, userId);
            if (note == null)
                throw new ArgumentException("Note not found");

            note.IsPinned = !note.IsPinned;
            return await _noteRepository.UpdateAsync(note);
        }

        public async Task<Note> ToggleArchiveNoteAsync(int id, int userId)
        {
            var note = await GetNoteByIdAsync(id, userId);
            if (note == null)
                throw new ArgumentException("Note not found");

            note.IsArchived = !note.IsArchived;
            return await _noteRepository.UpdateAsync(note);
        }
    }
}
