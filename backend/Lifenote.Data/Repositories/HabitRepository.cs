using Lifenote.Core.Interfaces;
using Lifenote.Core.Models;
using Lifenote.Data.Data;
using Microsoft.EntityFrameworkCore;

namespace Lifenote.Data.Repositories
{
    public class HabitRepository: IHabitRepository
    {
        private readonly LifenoteDbContext _context;

        public HabitRepository(LifenoteDbContext context)
        {
            _context = context;
        }

        public async Task<Habit?> GetByIdAsync(int id, int userId)
        {
            return await _context.Habits
                .Include(h => h.HabitStreak)
                .FirstOrDefaultAsync(h => h.Id == id && h.UserId == userId);
        }

        public async Task<IEnumerable<Habit>> GetAllAsync(int userId, bool includeInactive = false)
        {
            var query = _context.Habits
               .Include(h => h.HabitStreak)
               .Where(h => h.UserId == userId);

            var query2 = (from habit in _context.Habits where habit.Color.Contains("Green") select habit.Name).ToListAsync();

            if (!includeInactive)
            {
                query = query.Where(h => h.IsActive);
            }

            return await query
                .OrderByDescending(h => h.CreatedAt).ToListAsync();
        }

        public async Task<Habit> CreateAsync(Habit habit)
        {
            habit.CreatedAt = DateTime.UtcNow;
            habit.UpdatedAt = DateTime.UtcNow;

            if (habit.StartDate == default)
            {
                habit.StartDate = DateTime.UtcNow;
            }

            await _context.Habits.AddAsync(habit);
            return habit;
        }

        public async Task UpdateAsync(Habit habit)
        {
            var existingHabit = await _context.Habits.FindAsync(habit.Id);

            if (existingHabit != null && existingHabit.UserId == habit.UserId)
            {
                existingHabit.Name = habit.Name;
                existingHabit.Description = habit.Description;
                existingHabit.Color = habit.Color;
                existingHabit.IconName = habit.IconName;
                existingHabit.FrequencyType = habit.FrequencyType;
                existingHabit.FrequencyValue = habit.FrequencyValue;
                existingHabit.TargetCount = habit.TargetCount;
                existingHabit.IsActive = habit.IsActive;
                existingHabit.EndDate = habit.EndDate;
                existingHabit.UpdatedAt = DateTime.UtcNow;
            }
        }

        public async Task<bool> DeleteAsync(int id, int userId)
        {
            var habit = await _context.Habits
                .FirstOrDefaultAsync(h => h.Id == id && h.UserId == userId);

            if (habit == null) return false;

            _context.Habits.Remove(habit);
            return true;
        }

        // === Habit Logs ===

        public async Task<HabitLog> AddLogAsync(HabitLog log)
        {
            log.CreatedAt = DateTime.UtcNow;
            log.CompletedDate = log.CompletedAt.Date;

            await _context.HabitLogs.AddAsync(log);
            return log;
        }

        public async Task<HabitLog?> GetTodayLogAsync(int habitId, int userId, DateTime date)
        {
            return await _context.HabitLogs
                .FirstOrDefaultAsync(l =>
                    l.HabitId == habitId &&
                    l.UserId == userId &&
                    l.CompletedDate == date.Date);
        }

        public async Task<int> GetTodayLogCountAsync(int habitId, int userId, DateTime date)
        {
            return await _context.HabitLogs
                .CountAsync(l =>
                    l.HabitId == habitId &&
                    l.UserId == userId &&
                    l.CompletedDate == date.Date);
        }

        public async Task<bool> RemoveLogAsync(int logId, int userId)
        {
            var log = await _context.HabitLogs
                .FirstOrDefaultAsync(l => l.Id == logId && l.UserId == userId);

            if (log == null) return false;

            _context.HabitLogs.Remove(log);
            return true;
        }

        public async Task<IEnumerable<HabitLog>> GetLogsAsync(int habitId, int userId, DateTime? startDate = null, DateTime? endDate = null)
        {
            var query = _context.HabitLogs
                .Where(l => l.HabitId == habitId && l.UserId == userId);

            if (startDate.HasValue)
            {
                query = query.Where(l => l.CompletedDate >= startDate.Value.Date);
            }

            if (endDate.HasValue)
            {
                query = query.Where(l => l.CompletedDate <= endDate.Value.Date);
            }

            return await query
                .OrderByDescending(l => l.CompletedDate)
                .ToListAsync();
        }

        // === Streaks ===

        public async Task<HabitStreak?> GetStreakAsync(int habitId, int userId)
        {
            return await _context.HabitStreaks
                .FirstOrDefaultAsync(s => s.HabitId == habitId && s.UserId == userId);
        }

        public async Task UpdateStreakAsync(HabitStreak streak)
        {
            streak.CalculatedAt = DateTime.UtcNow;
            _context.HabitStreaks.Update(streak);
        }

        public async Task<HabitStreak> CreateStreakAsync(HabitStreak streak)
        {
            streak.CalculatedAt = DateTime.UtcNow;
            await _context.HabitStreaks.AddAsync(streak);
            return streak;
        }

        // === Queries ===

        public async Task<IEnumerable<Habit>> GetHabitsWithTodayStatusAsync(int userId, DateTime today)
        {
            var habits = await _context.Habits
                .Include(h => h.HabitStreak)
                .Include(h => h.HabitLogs.Where(l => l.CompletedDate == today.Date))
                .Where(h => h.UserId == userId && h.IsActive)
                .OrderByDescending(h => h.CreatedAt)
                .ToListAsync();

            return habits;
        }

        public async Task<bool> ExistsAsync(int id, int userId)
        {
            return await _context.Habits
                .AnyAsync(h => h.Id == id && h.UserId == userId);
        }
    }
}
