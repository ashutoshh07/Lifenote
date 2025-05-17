using System;
using System.Collections.Generic;

namespace Lifenote.Core.Entities;

public partial class Reminder
{
    public Guid Reminderid { get; set; }

    public Guid Userid { get; set; }

    public Guid? Noteid { get; set; }

    public string? Title { get; set; }

    public DateTime Remindertime { get; set; }

    public string? Recurring { get; set; }

    public DateTime Createdat { get; set; }

    public virtual Note? Note { get; set; }

    public virtual User User { get; set; } = null!;
}
