using System;
using System.Collections.Generic;

namespace Lifenote.Core.Models;

public partial class UserInfo
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Username { get; set; } = null!;

    public DateOnly? DateOfBirth { get; set; }

    public string? ProfilePicture { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<Habit> Habits { get; set; } = new List<Habit>();

    public virtual ICollection<Note> Notes { get; set; } = new List<Note>();

    public virtual ICollection<Timer> Timers { get; set; } = new List<Timer>();
}
