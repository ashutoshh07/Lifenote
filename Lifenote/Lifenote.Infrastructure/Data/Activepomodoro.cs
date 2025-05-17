using System;
using System.Collections.Generic;

namespace Lifenote.Core.Entities;

public partial class Activepomodoro
{
    public Guid Userid { get; set; }

    public string? Title { get; set; }

    public DateTime? Starttime { get; set; }

    public int? Duration { get; set; }

    public string? Type { get; set; }

    public bool? Isrunning { get; set; }

    public DateTime Updatedat { get; set; }

    public virtual User User { get; set; } = null!;
}
