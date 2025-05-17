using Lifenote.Application.DTOs;
using Lifenote.Core.Entities;
using Lifenote.Core.Interfaces;

namespace Lifenote.Application.Services;

public class ReminderService
{
    private readonly IGenericRepository<reminder> _reminderRepository;

    public ReminderService(IGenericRepository<reminder> reminderRepository)
    {
        _reminderRepository = reminderRepository;
    }

    public async Task<ReminderDto> CreateAsync(CreateReminderDto dto)
    {
        var reminder = new reminder
        {
            reminderid = Guid.NewGuid(),
            userid = dto.UserId,
            noteid = dto.NoteId,
            title = dto.Title,
            remindertime = dto.ReminderTime,
            recurring = dto.Recurring,
            createdat = DateTime.UtcNow
        };

        await _reminderRepository.AddAsync(reminder);
        return new ReminderDto(reminder.reminderid, reminder.userid, reminder.noteid, 
            reminder.title, reminder.remindertime, reminder.recurring, reminder.createdat);
    }

    public async Task<IEnumerable<ReminderDto>> GetUserRemindersAsync(Guid userId)
    {
        var reminders = await _reminderRepository.FindAsync(r => r.userid == userId);
        return reminders.Select(r => new ReminderDto(r.reminderid, r.userid, r.noteid,
            r.title, r.remindertime, r.recurring, r.createdat));
    }

    public async Task<ReminderDto?> GetByIdAsync(Guid id)
    {
        var reminder = await _reminderRepository.GetByIdAsync(id);
        return reminder == null ? null : new ReminderDto(reminder.reminderid, reminder.userid,
            reminder.noteid, reminder.title, reminder.remindertime, reminder.recurring, reminder.createdat);
    }

    public async Task UpdateAsync(Guid id, UpdateReminderDto dto)
    {
        var reminder = await _reminderRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Reminder with ID {id} not found.");

        reminder.title = dto.Title;
        reminder.remindertime = dto.ReminderTime;
        reminder.recurring = dto.Recurring;

        await _reminderRepository.UpdateAsync(reminder);
    }

    public async Task DeleteAsync(Guid id)
    {
        var reminder = await _reminderRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Reminder with ID {id} not found.");
        await _reminderRepository.DeleteAsync(reminder);
    }
}
