using System;
using System.Collections.Generic;

namespace Lifenote.Core.Models;

public partial class Habit
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string Frequency { get; set; } = null!;

    public int? TargetCount { get; set; }

    public int? CurrentStreak { get; set; }

    public int? LongestStreak { get; set; }

    public string? Color { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
