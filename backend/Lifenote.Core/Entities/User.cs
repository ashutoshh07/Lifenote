using System;
using System.Collections.Generic;

namespace Lifenote.Core.Entities;

public partial class User
{
    public Guid userid { get; set; }

    public string email { get; set; } = null!;

    public string username { get; set; } = null!;

    public string? phonenumber { get; set; }

    public DateTime createdat { get; set; }

    public virtual activepomodoro? activepomodoro { get; set; }

    public virtual ICollection<note> notes { get; set; } = new List<note>();

    public virtual ICollection<pomodorosession> pomodorosessions { get; set; } = new List<pomodorosession>();

    public virtual ICollection<reminder> reminders { get; set; } = new List<reminder>();
}
