using System;
using System.Collections.Generic;

namespace Lifenote.Core.Entities;

public partial class reminder
{
    public Guid reminderid { get; set; }

    public Guid userid { get; set; }

    public Guid? noteid { get; set; }

    public string? title { get; set; }

    public DateTime remindertime { get; set; }

    public string? recurring { get; set; }

    public DateTime createdat { get; set; }

    public virtual note? note { get; set; }

    public virtual User user { get; set; } = null!;
}
