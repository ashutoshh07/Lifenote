using System;
using System.Collections.Generic;

namespace Lifenote.Core.Models;

public partial class HabitLog
{
    public int Id { get; set; }

    public int HabitId { get; set; }

    public int UserId { get; set; }

    public DateTime CompletedAt { get; set; }

    public DateTime CompletedDate { get; set; }

    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Habit Habit { get; set; } = null!;

    public virtual UserInfo User { get; set; } = null!;
}
