using System;
using System.Collections.Generic;

namespace Lifenote.Core.Models;

public partial class Habit
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int TargetCount { get; set; }

    public string? Color { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public string IconName { get; set; } = null!;

    public string FrequencyType { get; set; } = null!;

    public string? FrequencyValue { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public virtual ICollection<HabitLog> HabitLogs { get; set; } = new List<HabitLog>();

    public virtual HabitStreak? HabitStreak { get; set; }
}
