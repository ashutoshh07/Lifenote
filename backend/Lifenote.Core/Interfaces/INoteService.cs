using Lifenote.Core.Models;

namespace Lifenote.Core.Interfaces
{
    public interface INoteService
    {
        Task<IEnumerable<Note>> GetAllNotesAsync(int userId);
        Task<Note?> GetNoteByIdAsync(int id, int userId);
        Task<Note> CreateNoteAsync(Note note);
        Task<Note> UpdateNoteAsync(Note note);
        Task<bool> DeleteNoteAsync(int id, int userId);
        Task<IEnumerable<Note>> GetNotesByCategoryAsync(int userId, string category);
        Task<IEnumerable<Note>> GetPinnedNotesAsync(int userId);
        Task<IEnumerable<Note>> SearchNotesAsync(int userId, string searchTerm);
        Task<Note> TogglePinNoteAsync(int id, int userId);
        Task<Note> ToggleArchiveNoteAsync(int id, int userId);
    }
}
