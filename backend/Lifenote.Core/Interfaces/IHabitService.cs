using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lifenote.Core.DTOs.Habit;

namespace Lifenote.Core.Interfaces
{
    public interface IHabitService
    {
        // CRUD Operations
        Task<HabitDto> CreateHabitAsync(int userId, CreateHabitDto dto);
        Task<HabitDto> GetHabitByIdAsync(int userId, int habitId);
        Task<IEnumerable<HabitDto>> GetUserHabitsAsync(int userId, bool includeInactive = false);
        Task<HabitDto> UpdateHabitAsync(int userId, int habitId, UpdateHabitDto dto);
        Task<bool> DeleteHabitAsync(int userId, int habitId);
        Task<bool> ToggleHabitStatusAsync(int userId, int habitId); // Pause/Resume

        // Check-in Operations
        Task<HabitLogDto> CheckInHabitAsync(int userId, CheckInDto dto);
        Task<bool> UndoCheckInAsync(int userId, int habitId);

        // History & Analytics
        Task<IEnumerable<HabitLogDto>> GetHabitHistoryAsync(int userId, int habitId, DateTime? startDate = null, DateTime? endDate = null);
        Task<HabitStatisticsDto> GetHabitStatisticsAsync(int userId, int habitId);
        Task<WeeklyCalendarDto> GetWeeklyCalendarAsync(int userId, DateTime weekStart);
    }
}
