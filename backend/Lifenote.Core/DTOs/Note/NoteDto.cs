namespace Lifenote.Core.DTOs.Note;

/// <summary>
/// Response DTO for a note; used for GET and mutation responses.
/// </summary>
public class NoteDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? Category { get; set; }
    public List<string>? Tags { get; set; }
    public bool IsPinned { get; set; }
    public bool IsArchived { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
