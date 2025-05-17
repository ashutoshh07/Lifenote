using System;
using System.Collections.Generic;

namespace Lifenote.Core.Entities;

public partial class Pomodorosession
{
    public Guid Sessionid { get; set; }

    public Guid Userid { get; set; }

    public string? Title { get; set; }

    public DateTime Starttime { get; set; }

    public DateTime Endtime { get; set; }

    public int Duration { get; set; }

    public string? Type { get; set; }

    public virtual User User { get; set; } = null!;
}
