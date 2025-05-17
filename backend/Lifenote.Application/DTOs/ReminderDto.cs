namespace Lifenote.Application.DTOs;

public record ReminderDto(
    Guid ReminderId,
    Guid UserId,
    Guid? NoteId,
    string? Title,
    DateTime ReminderTime,
    string? Recurring,
    DateTime CreatedAt
);

public record CreateReminderDto(
    Guid UserId,
    Guid? NoteId,
    string? Title,
    DateTime ReminderTime,
    string? Recurring
);

public record UpdateReminderDto(
    string? Title,
    DateTime ReminderTime,
    string? Recurring
);
