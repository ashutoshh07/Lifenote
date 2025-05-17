using Lifenote.Application.DTOs;
using Lifenote.Core.Entities;
using Lifenote.Core.Interfaces;

namespace Lifenote.Application.Services;

public class UserService
{
    private readonly IGenericRepository<User> _userRepository;

    public UserService(IGenericRepository<User> userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserDto> CreateAsync(CreateUserDto dto)
    {
        var user = new User
        {
            userid = Guid.NewGuid(),
            email = dto.Email,
            username = dto.Username,
            phonenumber = dto.PhoneNumber,
            createdat = DateTime.UtcNow
        };

        await _userRepository.AddAsync(user);
        return new UserDto(user.userid, user.email, user.username, user.phonenumber, user.createdat);
    }

    public async Task<UserDto?> GetByIdAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        return user == null ? null 
            : new UserDto(user.userid, user.email, user.username, user.phonenumber, user.createdat);
    }

    public async Task<IEnumerable<UserDto>> GetAllAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return users.Select(u => new UserDto(u.userid, u.email, u.username, u.phonenumber, u.createdat));
    }

    public async Task UpdateAsync(Guid id, UpdateUserDto dto)
    {
        var user = await _userRepository.GetByIdAsync(id) 
            ?? throw new KeyNotFoundException($"User with ID {id} not found");

        user.email = dto.Email;
        user.username = dto.Username;
        user.phonenumber = dto.PhoneNumber;

        await _userRepository.UpdateAsync(user);
    }

    public async Task DeleteAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"User with ID {id} not found");
        
        await _userRepository.DeleteAsync(user);
    }
}
