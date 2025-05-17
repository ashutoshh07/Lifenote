namespace Lifenote.Application.DTOs;

public record NoteDto(
    Guid NoteId,
    Guid UserId,
    string? Title,
    string? Content,
    string? Type,
    string? ColorTag,
    bool? Pinned,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);

public record CreateNoteDto(
    Guid UserId,
    string? Title,
    string? Content,
    string? Type,
    string? ColorTag,
    bool? Pinned = false
);

public record UpdateNoteDto(
    string? Title,
    string? Content,
    string? Type,
    string? ColorTag,
    bool? Pinned
);
