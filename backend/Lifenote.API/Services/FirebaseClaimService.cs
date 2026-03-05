using FirebaseAdmin.Auth;

namespace Lifenote.API.Services;

public class FirebaseClaimService : IFirebaseClaimService
{
    public async Task SetAppUserIdClaimAsync(string firebaseUid, int appUserId, CancellationToken cancellationToken = default)
    {
        try
        {
            var auth = FirebaseAuth.DefaultInstance;
            if (auth == null)
                return;

            var claims = new Dictionary<string, object>
            {
                { "app_user_id", appUserId }
            };
            await auth.SetCustomUserClaimsAsync(firebaseUid, claims, cancellationToken);
        }
        catch (Exception)
        {
            // If Firebase is not configured or claim set fails, skip; fallback (cache/DB) will still work
        }
    }
}
