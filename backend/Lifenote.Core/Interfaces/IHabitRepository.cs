using Lifenote.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lifenote.Core.Interfaces
{
    public interface IHabitRepository
    {
        // CRUD
        Task<Habit?> GetByIdAsync(int id, int userId);
        Task<IEnumerable<Habit>> GetAllAsync(int userId, bool includeInactive = false);
        Task<Habit> CreateAsync(Habit habit);
        Task UpdateAsync(Habit habit);
        Task<bool> DeleteAsync(int id, int userId);

        // Habit Logs
        Task<HabitLog> AddLogAsync(HabitLog log);
        Task<HabitLog?> GetTodayLogAsync(int habitId, int userId, DateTime date);
        Task<int> GetTodayLogCountAsync(int habitId, int userId, DateTime date);
        Task<bool> RemoveLogAsync(int logId, int userId);
        Task<IEnumerable<HabitLog>> GetLogsAsync(int habitId, int userId, DateTime? startDate = null, DateTime? endDate = null);

        // Streaks
        Task<HabitStreak?> GetStreakAsync(int habitId, int userId);
        Task UpdateStreakAsync(HabitStreak streak);
        Task<HabitStreak> CreateStreakAsync(HabitStreak streak);

        // Queries for analytics
        Task<IEnumerable<Habit>> GetHabitsWithTodayStatusAsync(int userId, DateTime today);
        Task<bool> ExistsAsync(int id, int userId);
    }
}
