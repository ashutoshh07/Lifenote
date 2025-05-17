using System;
using System.Collections.Generic;

namespace Lifenote.Core.Entities;

public partial class pomodorosession
{
    public Guid sessionid { get; set; }

    public Guid userid { get; set; }

    public string? title { get; set; }

    public DateTime starttime { get; set; }

    public DateTime endtime { get; set; }

    public int duration { get; set; }

    public string? type { get; set; }

    public virtual User user { get; set; } = null!;
}
