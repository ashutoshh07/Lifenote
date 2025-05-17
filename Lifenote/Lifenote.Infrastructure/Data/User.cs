using System;
using System.Collections.Generic;

namespace Lifenote.Core.Entities;

public partial class User
{
    public Guid Userid { get; set; }

    public string Email { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string? Phonenumber { get; set; }

    public DateTime Createdat { get; set; }

    public virtual Activepomodoro? Activepomodoro { get; set; }

    public virtual ICollection<Note> Notes { get; set; } = new List<Note>();

    public virtual ICollection<Pomodorosession> Pomodorosessions { get; set; } = new List<Pomodorosession>();

    public virtual ICollection<Reminder> Reminders { get; set; } = new List<Reminder>();
}
