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

    public DateTime? DateOfBirth { get; set; }

    public string? ProfilePicture { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool? IsActive { get; set; }

    public string AuthProviderId { get; set; } = null!;

    public string? Bio { get; set; }

    public string? Theme { get; set; }

    public DateTime? LastLoginAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual ICollection<HabitLog> HabitLogs { get; set; } = new List<HabitLog>();

    public virtual ICollection<HabitStreak> HabitStreaks { get; set; } = new List<HabitStreak>();
}
