using System;
using System.Collections.Generic;

namespace Lifenote.Core.Entities;

public partial class Checklistitem
{
    public Guid Itemid { get; set; }

    public Guid Noteid { get; set; }

    public string? Content { get; set; }

    public bool? Isdone { get; set; }

    public virtual Note Note { get; set; } = null!;
}
