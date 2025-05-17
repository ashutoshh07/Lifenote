namespace Lifenote.Application.DTOs;

public class NoteDto
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
}

public class CreateNoteDto
{
    public Guid Userid { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }
    public string? Type { get; set; }
    public string? Colortag { get; set; }
    public bool? Pinned { get; set; }
}
