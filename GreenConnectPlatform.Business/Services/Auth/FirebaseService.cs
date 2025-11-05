using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Logging;

namespace GreenConnectPlatform.Business.Services.Auth;

public class FirebaseService
{
    private readonly ILogger<FirebaseService> _logger;

    public FirebaseService(ILogger<FirebaseService> logger)
    {
        _logger = logger;

        if (FirebaseApp.DefaultInstance == null)
            FirebaseApp.Create(new AppOptions
            {
                Credential = GoogleCredential.FromFile("Configs:firebase-service-account.json")
            });
    }

    public async Task<FirebaseToken?> VerifyFirebaseTokenAsync(string idToken)
    {
        try
        {
            var decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(idToken);
            return decodedToken;
        }
        catch (Exception ex)
        {
            _logger.LogWarning("Firebase token verification failed: {Message}", ex.Message);
            return null;
        }
    }
}