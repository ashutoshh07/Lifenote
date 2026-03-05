namespace Lifenote.Core.Interfaces;

/// <summary>
/// Resolves the current request's app user id (integer) from the auth token.
/// Uses JWT claim "app_user_id" when present (Firebase custom claim), with cache/DB fallback to avoid DB on every request.
/// </summary>
public interface ICurrentUserService
{
    /// <summary>
    /// Gets the current user's app database id. Throws if not authenticated or user not found.
    /// </summary>
    Task<int> GetCurrentUserIdAsync(CancellationToken cancellationToken = default);
}
