using System;
using System.Collections.Generic;

namespace Lifenote.Core.Entities;

public partial class activepomodoro
{
    public Guid userid { get; set; }

    public string? title { get; set; }

    public DateTime? starttime { get; set; }

    public int? duration { get; set; }

    public string? type { get; set; }

    public bool? isrunning { get; set; }

    public DateTime updatedat { get; set; }

    public virtual User user { get; set; } = null!;
}
