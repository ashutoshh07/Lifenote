using System;
using System.Collections.Generic;

namespace Lifenote.Core.Entities;

public partial class Note
{
    public Guid Noteid { get; set; }

    public Guid Userid { get; set; }

    public string? Title { get; set; }

    public string? Content { get; set; }

    public string? Type { get; set; }

    public string? Colortag { get; set; }

    public bool? Pinned { get; set; }

    public DateTime Createdat { get; set; }

    public DateTime? Updatedat { get; set; }

    public virtual ICollection<Checklistitem> Checklistitems { get; set; } = new List<Checklistitem>();

    public virtual ICollection<Reminder> Reminders { get; set; } = new List<Reminder>();

    public virtual User User { get; set; } = null!;
}
