using Lifenote.Core.Interfaces;
using Lifenote.Core.Models;

namespace Lifenote.Data.Services
{
    public class NoteService : INoteService
    {
        private readonly IUnitOfWork _unitOfWork;

        public NoteService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Note>> GetAllNotesAsync(int userId)
        {
            return await _unitOfWork.Notes.GetAllAsync(userId);
        }

        public async Task<Note?> GetNoteByIdAsync(int id, int userId)
        {
            var note = await _unitOfWork.Notes.GetByIdAsync(id);
            if (note == null || note.UserId != userId) return null;
            return note;
        }

        public async Task<Note> CreateNoteAsync(Note note)
        {
            if (string.IsNullOrWhiteSpace(note.Title))
                throw new ArgumentException("Note title cannot be empty");

            if (string.IsNullOrWhiteSpace(note.Content))
                throw new ArgumentException("Note content cannot be empty");

            await _unitOfWork.Notes.AddAsync(note);
            await _unitOfWork.SaveChangesAsync();  // ✅ Single transaction

            return note;
        }

        public async Task<Note> UpdateNoteAsync(Note note)
        {
            if (string.IsNullOrWhiteSpace(note.Title))
                throw new ArgumentException("Note title cannot be empty");

            if (string.IsNullOrWhiteSpace(note.Content))
                throw new ArgumentException("Note content cannot be empty");

            var existingNote = await _unitOfWork.Notes.GetByIdAsync(note.Id);
            if (existingNote == null || existingNote.UserId != note.UserId)
                throw new UnauthorizedAccessException("Note not found or access denied");

            _unitOfWork.Notes.Update(note);
            await _unitOfWork.SaveChangesAsync();  // ✅ Single transaction

            return note;
        }

        public async Task<bool> DeleteNoteAsync(int id, int userId)
        {
            if (!await _unitOfWork.Notes.ExistsAsync(id, userId))
                return false;

            await _unitOfWork.Notes.RemoveAsync(id);
            await _unitOfWork.SaveChangesAsync();  // ✅ Single transaction

            return true;
        }

        public async Task<IEnumerable<Note>> GetNotesByCategoryAsync(int userId, string category)
        {
            return await _unitOfWork.Notes.GetByCategoryAsync(userId, category);
        }

        public async Task<IEnumerable<Note>> GetPinnedNotesAsync(int userId)
        {
            return await _unitOfWork.Notes.GetPinnedAsync(userId);
        }

        public async Task<IEnumerable<Note>> SearchNotesAsync(int userId, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetAllNotesAsync(userId);

            return await _unitOfWork.Notes.SearchAsync(userId, searchTerm);
        }

        public async Task<Note> TogglePinNoteAsync(int id, int userId)
        {
            var note = await GetNoteByIdAsync(id, userId);
            if (note == null)
                throw new ArgumentException("Note not found");

            note.IsPinned = !note.IsPinned;
            _unitOfWork.Notes.Update(note);
            await _unitOfWork.SaveChangesAsync();

            return note;
        }

        public async Task<Note> ToggleArchiveNoteAsync(int id, int userId)
        {
            var note = await GetNoteByIdAsync(id, userId);
            if (note == null)
                throw new ArgumentException("Note not found");

            note.IsArchived = !note.IsArchived;
            _unitOfWork.Notes.Update(note);
            await _unitOfWork.SaveChangesAsync();

            return note;
        }
    }
}
