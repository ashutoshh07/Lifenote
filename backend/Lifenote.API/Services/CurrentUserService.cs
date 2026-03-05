using System.Security.Claims;
using Lifenote.Core.Interfaces;
using Lifenote.Core.DTOs.UserInfo;
using Microsoft.Extensions.Caching.Memory;

namespace Lifenote.API.Services;

/// <summary>
/// Resolves current app user id from JWT: "app_user_id" claim first (Firebase custom claim),
/// then in-memory cache (Firebase UID -> app user id), then single DB lookup with cache + optional claim set.
/// </summary>
public class CurrentUserService : ICurrentUserService
{
    public const string AppUserIdClaim = "app_user_id";
    public const string CacheKeyPrefix = "uid:";
    public static readonly TimeSpan CacheExpiration = TimeSpan.FromMinutes(15);

    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserInfoService _userInfoService;
    private readonly IMemoryCache _cache;
    private readonly IFirebaseClaimService _firebaseClaimService;

    public CurrentUserService(
        IHttpContextAccessor httpContextAccessor,
        IUserInfoService userInfoService,
        IMemoryCache cache,
        IFirebaseClaimService firebaseClaimService)
    {
        _httpContextAccessor = httpContextAccessor;
        _userInfoService = userInfoService;
        _cache = cache;
        _firebaseClaimService = firebaseClaimService;
    }

    public async Task<int> GetCurrentUserIdAsync(CancellationToken cancellationToken = default)
    {
        var user = _httpContextAccessor.HttpContext?.User
            ?? throw new UnauthorizedAccessException("Not authenticated");

        // 1. From token claim (set via Firebase custom claim; no DB, no cache)
        var appUserIdClaim = user.FindFirst(AppUserIdClaim)?.Value;
        if (!string.IsNullOrEmpty(appUserIdClaim) && int.TryParse(appUserIdClaim, out int fromClaim))
            return fromClaim;

        // 2. From cache (Firebase UID -> app user id)
        var firebaseUid = user.FindFirst("user_id")?.Value ?? user.FindFirst("sub")?.Value;
        if (string.IsNullOrEmpty(firebaseUid))
            throw new UnauthorizedAccessException("Invalid token: missing user identifier");

        var cacheKey = CacheKeyPrefix + firebaseUid;
        if (_cache.TryGetValue(cacheKey, out int cachedId))
            return cachedId;

        // 3. From DB (once per user per cache TTL), then cache and set custom claim for next token
        UserProfileResponse profile;
        try
        {
            profile = await _userInfoService.GetUserByAuthIdAsync(firebaseUid);
        }
        catch (KeyNotFoundException)
        {
            throw new UnauthorizedAccessException("User not found");
        }

        var appUserId = profile.Id;
        _cache.Set(cacheKey, appUserId, CacheExpiration);
        await _firebaseClaimService.SetAppUserIdClaimAsync(firebaseUid, appUserId, cancellationToken);

        return appUserId;
    }
}
