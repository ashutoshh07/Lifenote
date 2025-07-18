using Lifenote.Application.DTOs;
using Lifenote.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Lifenote.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PomodorosController : ControllerBase
{
    private readonly IPomodoroService _pomodoroService;

    public PomodorosController(IPomodoroService pomodoroService)
    {
        _pomodoroService = pomodoroService;
    }

    [HttpGet("user/{userId}")]
    public async Task<ActionResult<IEnumerable<PomodoroSessionDto>>> GetPomodoroSessionsByUser(Guid userId)
    {
        var sessions = await _pomodoroService.GetPomodoroSessionsByUser(userId);
        return Ok(sessions);
    }

    [HttpPost]
    public async Task<ActionResult<PomodoroSessionDto>> CreatePomodoroSession(CreatePomodoroSessionDto createDto)
    {
        var newSession = await _pomodoroService.CreatePomodoroSession(createDto);
        return CreatedAtAction(nameof(GetPomodoroSessionsByUser), new { userId = newSession.UserId }, newSession);
    }

    [HttpGet("active/user/{userId}")]
    public async Task<ActionResult<ActivePomodoroDto>> GetActivePomodoro(Guid userId)
    {
        var activePomodoro = await _pomodoroService.GetActivePomodoro(userId);
        if (activePomodoro == null)
        {
            return NotFound();
        }
        return Ok(activePomodoro);
    }

    [HttpPost("active")]
    public async Task<ActionResult<ActivePomodoroDto>> SaveActivePomodoro(ActivePomodoroDto activePomodoroDto)
    {
        var savedPomodoro = await _pomodoroService.SaveActivePomodoro(activePomodoroDto);
        return Ok(savedPomodoro);
    }

    [HttpPut("active/user/{userId}")]
    public async Task<IActionResult> UpdateActivePomodoro(Guid userId, UpdateActivePomodoroDto updateDto)
    {
        await _pomodoroService.UpdateActivePomodoro(userId, updateDto);
        return NoContent();
    }
}
