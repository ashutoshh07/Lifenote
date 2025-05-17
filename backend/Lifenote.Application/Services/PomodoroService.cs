using Lifenote.Application.DTOs;
using Lifenote.Core.Entities;
using Lifenote.Core.Interfaces;

namespace Lifenote.Application.Services;

public class PomodoroService
{
    private readonly IGenericRepository<pomodorosession> _sessionRepository;
    private readonly IGenericRepository<activepomodoro> _activeRepository;

    public PomodoroService(
        IGenericRepository<pomodorosession> sessionRepository,
        IGenericRepository<activepomodoro> activeRepository)
    {
        _sessionRepository = sessionRepository;
        _activeRepository = activeRepository;
    }

    public async Task<PomodoroSessionDto> CreateSessionAsync(CreatePomodoroSessionDto dto)
    {
        var session = new pomodorosession
        {
            sessionid = Guid.NewGuid(),
            userid = dto.UserId,
            title = dto.Title,
            starttime = dto.StartTime,
            endtime = dto.EndTime,
            duration = dto.Duration,
            type = dto.Type
        };

        await _sessionRepository.AddAsync(session);
        return new PomodoroSessionDto(session.sessionid, session.userid, session.title,
            session.starttime, session.endtime, session.duration, session.type);
    }

    public async Task<ActivePomodoroDto?> GetActiveSessionAsync(Guid userId)
    {
        var active = await _activeRepository.GetByIdAsync(userId);
        return active == null ? null : new ActivePomodoroDto(active.userid, active.title,
            active.starttime, active.duration, active.type, active.isrunning, active.updatedat);
    }

    public async Task UpdateActiveSessionAsync(Guid userId, UpdateActivePomodoroDto dto)
    {
        var active = await _activeRepository.GetByIdAsync(userId);
        
        if (active == null)
        {
            active = new activepomodoro
            {
                userid = userId,
                title = dto.Title,
                starttime = dto.StartTime,
                duration = dto.Duration,
                type = dto.Type,
                isrunning = dto.IsRunning,
                updatedat = DateTime.UtcNow
            };
            await _activeRepository.AddAsync(active);
        }
        else
        {
            active.title = dto.Title;
            active.starttime = dto.StartTime;
            active.duration = dto.Duration;
            active.type = dto.Type;
            active.isrunning = dto.IsRunning;
            active.updatedat = DateTime.UtcNow;
            await _activeRepository.UpdateAsync(active);
        }
    }

    public async Task<IEnumerable<PomodoroSessionDto>> GetUserSessionsAsync(Guid userId)
    {
        var sessions = await _sessionRepository.FindAsync(s => s.userid == userId);
        return sessions.Select(s => new PomodoroSessionDto(s.sessionid, s.userid, s.title,
            s.starttime, s.endtime, s.duration, s.type));
    }
}
