using Lifenote.Core.DTOs;
using Lifenote.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Lifenote.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserInfoController : ControllerBase
    {
        private readonly IUserInfoService _userInfoService;

        public UserInfoController(IUserInfoService userService)
        {
            _userInfoService = userService;
        }

        private string GetFirebaseUid()
        {
            return User.FindFirst("user_id")?.Value
                ?? throw new UnauthorizedAccessException("Invalid token");
        }

        private string? GetEmailFromToken()
        {
            return User.FindFirst(ClaimTypes.Email)?.Value;
        }

        [HttpPost]
        public async Task<ActionResult<UserProfileResponse>> CreateUser([FromBody] CreateUserDto request)
        {
            var firebaseUid = GetFirebaseUid();
            var email = GetEmailFromToken() ?? throw new UnauthorizedAccessException("Email missing from token");

            var user = await _userInfoService.CreateUserAsync(firebaseUid, email, request);
            return CreatedAtAction(nameof(GetCurrentUser), new { id = user.Id }, user);
        }

        [HttpGet("me")]
        public async Task<ActionResult<UserProfileResponse>> GetCurrentUser()
        {
            var firebaseUid = GetFirebaseUid();
            var user = await _userInfoService.GetUserByAuthIdAsync(firebaseUid);
            return Ok(user);
        }

        [HttpPut("me")]
        public async Task<ActionResult<UserProfileResponse>> UpdateProfile([FromBody] UpdateProfileDto request)
        {
            var firebaseUid = GetFirebaseUid();
            var user = await _userInfoService.UpdateProfileAsync(firebaseUid, request);
            return Ok(user);
        }

        [HttpPatch("me/theme")]
        public async Task<IActionResult> UpdateTheme([FromBody] UpdateThemeDto request)
        {
            var firebaseUid = GetFirebaseUid();
            await _userInfoService.UpdateThemeAsync(firebaseUid, request.Theme);
            return NoContent();
        }

        [HttpPatch("me/profile-picture")]
        public async Task<IActionResult> UpdateProfilePicture([FromBody] string profilePictureUrl)
        {
            var firebaseUid = GetFirebaseUid();
            await _userInfoService.UpdateProfilePictureAsync(firebaseUid, profilePictureUrl);
            return NoContent();
        }

        [HttpPatch("me/last-login")]
        public async Task<IActionResult> UpdateLastLogin()
        {
            var firebaseUid = GetFirebaseUid();
            await _userInfoService.UpdateLastLoginAsync(firebaseUid);
            return NoContent();
        }

        [HttpDelete("me")]
        public async Task<IActionResult> DeactivateAccount()
        {
            var firebaseUid = GetFirebaseUid();
            await _userInfoService.DeactivateUserAsync(firebaseUid);
            return NoContent();
        }

        [HttpGet("check-username/{username}")]
        [AllowAnonymous]
        public async Task<ActionResult<object>> CheckUsername(string username)
        {
            var isAvailable = await _userInfoService.IsUsernameAvailableAsync(username);
            return Ok(new { available = isAvailable });
        }
    }
}
