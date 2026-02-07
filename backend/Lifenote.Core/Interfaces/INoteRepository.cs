using Lifenote.Core.Models;

namespace Lifenote.Core.Interfaces
{
    public interface INoteRepository
    {
        Task<IEnumerable<Note>> GetAllAsync(int userId);
        Task<Note?> GetByIdAsync(int id);
        Task AddAsync(Note note);
        void Update(Note note);
        Task RemoveAsync(int id);
        Task<Note> CreateAsync(Note note);
        Task<Note> UpdateAsync(Note note);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<Note>> GetByCategoryAsync(int userId, string category);
        Task<IEnumerable<Note>> GetPinnedAsync(int userId);
        Task<IEnumerable<Note>> SearchAsync(int userId, string searchTerm);
        Task<bool> ExistsAsync(int id, int userId);
    }
}
