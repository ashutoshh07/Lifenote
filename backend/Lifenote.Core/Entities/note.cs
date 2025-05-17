using System;
using System.Collections.Generic;

namespace Lifenote.Core.Entities;

public partial class note
{
    public Guid noteid { get; set; }

    public Guid userid { get; set; }

    public string? title { get; set; }

    public string? content { get; set; }

    public string? type { get; set; }

    public string? colortag { get; set; }

    public bool? pinned { get; set; }

    public DateTime createdat { get; set; }

    public DateTime? updatedat { get; set; }

    public virtual ICollection<checklistitem> checklistitems { get; set; } = new List<checklistitem>();

    public virtual ICollection<reminder> reminders { get; set; } = new List<reminder>();

    public virtual User user { get; set; } = null!;
}
