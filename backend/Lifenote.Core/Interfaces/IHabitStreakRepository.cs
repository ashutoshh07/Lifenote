using Lifenote.Core.Models;

namespace Lifenote.Core.Interfaces;

public interface IHabitStreakRepository
{
    Task<HabitStreak?> GetByHabitIdAsync(int habitId, int userId);
    Task<HabitStreak?> GetByIdAsync(int id, int userId);
    Task<IEnumerable<HabitStreak>> GetAllByUserIdAsync(int userId);
    Task<HabitStreak> CreateAsync(HabitStreak streak);
    Task UpdateAsync(HabitStreak streak);
    Task<bool> DeleteAsync(int id, int userId);
    Task<bool> ExistsAsync(int habitId, int userId);
    Task<IEnumerable<HabitStreak>> GetTopStreaksByUserAsync(int userId, int topN = 5);
}
