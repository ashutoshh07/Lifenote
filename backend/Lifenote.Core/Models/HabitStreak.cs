using System;
using System.Collections.Generic;

namespace Lifenote.Core.Models;

public partial class HabitStreak
{
    public int Id { get; set; }

    public int HabitId { get; set; }

    public int UserId { get; set; }

    public int CurrentStreak { get; set; }

    public int LongestStreak { get; set; }

    public int TotalCompletions { get; set; }

    public DateTime? LastCompletedDate { get; set; }

    public DateTime CalculatedAt { get; set; }

    public virtual Habit Habit { get; set; } = null!;

    public virtual UserInfo User { get; set; } = null!;
}
