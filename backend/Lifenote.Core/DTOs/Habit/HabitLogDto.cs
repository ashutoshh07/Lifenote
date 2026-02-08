using System;
using System.Collections.Generic;
using System.Text;

namespace Lifenote.Core.DTOs.Habit
{
    public class HabitLogDto
    {
        public int Id { get; set; }
        public int HabitId { get; set; }
        public string HabitName { get; set; } = string.Empty;
        public DateTime CompletedAt { get; set; }
        public DateTime CompletedDate { get; set; }
        public string? Notes { get; set; }
        public int CurrentStreak { get; set; } // Streak after this check-in
    }
}
