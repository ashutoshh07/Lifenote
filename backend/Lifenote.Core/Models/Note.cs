using System;
using System.Collections.Generic;

namespace Lifenote.Core.Models;

public partial class Note
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;

    public string? Category { get; set; }

    public List<string>? Tags { get; set; }

    public bool? IsPinned { get; set; }

    public bool? IsArchived { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
