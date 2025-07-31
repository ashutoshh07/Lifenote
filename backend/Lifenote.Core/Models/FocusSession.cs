using System;
using System.Collections.Generic;

namespace Lifenote.Core.Models;

public partial class FocusSession
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string SessionType { get; set; } = null!;

    public int Duration { get; set; }

    public int? ActualDuration { get; set; }

    public DateTime? StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public bool? IsCompleted { get; set; }

    public string? Notes { get; set; }

    public DateTime? CreatedAt { get; set; }
}
