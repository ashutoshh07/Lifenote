namespace Lifenote.Application.DTOs;

public record PomodoroSessionDto(
    Guid SessionId,
    Guid UserId,
    string? Title,
    DateTime StartTime,
    DateTime EndTime,
    int Duration,
    string? Type
);

public record CreatePomodoroSessionDto(
    Guid UserId,
    string? Title,
    DateTime StartTime,
    DateTime EndTime,
    int Duration,
    string? Type
);

public record ActivePomodoroDto(
    Guid UserId,
    string? Title,
    DateTime? StartTime,
    int? Duration,
    string? Type,
    bool? IsRunning,
    DateTime UpdatedAt
);

public record UpdateActivePomodoroDto(
    string? Title,
    DateTime? StartTime,
    int? Duration,
    string? Type,
    bool? IsRunning
);
