using System;
using System.Collections.Generic;

namespace Lifenote.Core.Entities;

public partial class checklistitem
{
    public Guid itemid { get; set; }

    public Guid noteid { get; set; }

    public string? content { get; set; }

    public bool? isdone { get; set; }

    public virtual note note { get; set; } = null!;
}
