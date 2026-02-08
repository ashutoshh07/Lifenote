using Lifenote.Core.DTOs.Habit;
using Lifenote.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Lifenote.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class HabitsController : ControllerBase
    {
        private readonly IHabitService _habitService;

        public HabitsController(IHabitService habitService)
        {
            _habitService = habitService;
        }

        // ===== CRUD OPERATIONS =====

        /// <summary>
        /// Get all habits for the authenticated user
        /// </summary>
        /// <param name="includeInactive">Include paused/inactive habits</param>
        /// <returns>List of habits</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<HabitDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<HabitDto>>> GetHabits([FromQuery] bool includeInactive = false)
        {
            var userId = GetUserId();
            var habits = await _habitService.GetUserHabitsAsync(userId, includeInactive);
            return Ok(habits);
        }

        /// <summary>
        /// Get a specific habit by ID
        /// </summary>
        /// <param name="id">Habit ID</param>
        /// <returns>Habit details</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(HabitDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<HabitDto>> GetHabit(int id)
        {
            try
            {
                var userId = GetUserId();
                var habit = await _habitService.GetHabitByIdAsync(userId, id);
                return Ok(habit);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Habit not found" });
            }
        }

        /// <summary>
        /// Create a new habit
        /// </summary>
        /// <param name="dto">Habit creation data</param>
        /// <returns>Created habit</returns>
        [HttpPost]
        [ProducesResponseType(typeof(HabitDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<HabitDto>> CreateHabit([FromBody] CreateHabitDto dto)
        {
            try
            {
                var userId = GetUserId();
                var habit = await _habitService.CreateHabitAsync(userId, dto);
                return CreatedAtAction(nameof(GetHabit), new { id = habit.Id }, habit);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Update an existing habit
        /// </summary>
        /// <param name="id">Habit ID</param>
        /// <param name="dto">Updated habit data</param>
        /// <returns>Updated habit</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(HabitDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<HabitDto>> UpdateHabit(int id, [FromBody] UpdateHabitDto dto)
        {
            try
            {
                var userId = GetUserId();
                var habit = await _habitService.UpdateHabitAsync(userId, id, dto);
                return Ok(habit);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Habit not found" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Delete a habit permanently
        /// </summary>
        /// <param name="id">Habit ID</param>
        /// <returns>Success status</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteHabit(int id)
        {
            var userId = GetUserId();
            var result = await _habitService.DeleteHabitAsync(userId, id);

            if (!result)
                return NotFound(new { message = "Habit not found" });

            return NoContent();
        }

        /// <summary>
        /// Toggle habit active status (pause/resume)
        /// </summary>
        /// <param name="id">Habit ID</param>
        /// <returns>Success status</returns>
        [HttpPatch("{id}/toggle")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ToggleHabitStatus(int id)
        {
            var userId = GetUserId();
            var result = await _habitService.ToggleHabitStatusAsync(userId, id);

            if (!result)
                return NotFound(new { message = "Habit not found" });

            return Ok(new { message = "Habit status toggled successfully" });
        }

        // ===== CHECK-IN OPERATIONS =====

        /// <summary>
        /// Check in for a habit (mark as completed today)
        /// </summary>
        /// <param name="dto">Check-in data with habit ID and optional notes</param>
        /// <returns>Check-in log with updated streak</returns>
        [HttpPost("checkin")]
        [ProducesResponseType(typeof(HabitLogDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<HabitLogDto>> CheckIn([FromBody] CheckInDto dto)
        {
            try
            {
                var userId = GetUserId();
                var log = await _habitService.CheckInHabitAsync(userId, dto);
                return CreatedAtAction(nameof(GetHabitHistory), new { id = dto.HabitId }, log);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Habit not found" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Undo today's check-in for a habit
        /// </summary>
        /// <param name="id">Habit ID</param>
        /// <returns>Success status</returns>
        [HttpDelete("{id}/checkin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UndoCheckIn(int id)
        {
            var userId = GetUserId();
            var result = await _habitService.UndoCheckInAsync(userId, id);

            if (!result)
                return NotFound(new { message = "No check-in found for today" });

            return Ok(new { message = "Check-in removed successfully" });
        }

        // ===== HISTORY & ANALYTICS =====

        /// <summary>
        /// Get check-in history for a habit
        /// </summary>
        /// <param name="id">Habit ID</param>
        /// <param name="startDate">Optional start date filter</param>
        /// <param name="endDate">Optional end date filter</param>
        /// <returns>List of habit logs</returns>
        [HttpGet("{id}/history")]
        [ProducesResponseType(typeof(IEnumerable<HabitLogDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<HabitLogDto>>> GetHabitHistory(
            int id,
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null)
        {
            try
            {
                var userId = GetUserId();
                var logs = await _habitService.GetHabitHistoryAsync(userId, id, startDate, endDate);
                return Ok(logs);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Habit not found" });
            }
        }

        /// <summary>
        /// Get detailed statistics for a habit
        /// </summary>
        /// <param name="id">Habit ID</param>
        /// <returns>Habit statistics including streaks, completion rate, and best/worst days</returns>
        [HttpGet("{id}/statistics")]
        [ProducesResponseType(typeof(HabitStatisticsDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<HabitStatisticsDto>> GetHabitStatistics(int id)
        {
            try
            {
                var userId = GetUserId();
                var stats = await _habitService.GetHabitStatisticsAsync(userId, id);
                return Ok(stats);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Habit not found" });
            }
        }

        /// <summary>
        /// Get weekly calendar view of all habits
        /// </summary>
        /// <param name="weekStart">Start date of the week (defaults to current week's Monday)</param>
        /// <returns>Weekly calendar with completion status for all habits</returns>
        [HttpGet("calendar/weekly")]
        [ProducesResponseType(typeof(WeeklyCalendarDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<WeeklyCalendarDto>> GetWeeklyCalendar([FromQuery] DateTime? weekStart = null)
        {
            var userId = GetUserId();
            var start = weekStart ?? GetStartOfWeek(DateTime.UtcNow);
            var calendar = await _habitService.GetWeeklyCalendarAsync(userId, start);
            return Ok(calendar);
        }

        // ===== HELPER METHODS =====

        private int GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                           ?? User.FindFirst("sub")?.Value
                           ?? User.FindFirst("uid")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                throw new UnauthorizedAccessException("User ID not found in token");
            }

            return userId;
        }

        private DateTime GetStartOfWeek(DateTime date)
        {
            int diff = (7 + (date.DayOfWeek - DayOfWeek.Monday)) % 7;
            return date.AddDays(-1 * diff).Date;
        }
    }
}
