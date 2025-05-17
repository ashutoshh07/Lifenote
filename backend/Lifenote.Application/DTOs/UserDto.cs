namespace Lifenote.Application.DTOs;

public record UserDto(
    Guid UserId, 
    string Email, 
    string Username, 
    string? PhoneNumber, 
    DateTime CreatedAt
);

public record CreateUserDto(
    string Email,
    string Username,
    string? PhoneNumber
);

public record UpdateUserDto(
    string Email,
    string Username,
    string? PhoneNumber
);
