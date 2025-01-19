using Google.Apis.Auth;
using Server.Services.Interfaces;

namespace Server.Services.Implementations
{
    public class GoogleTokenValidator : IGoogleTokenValidator
    {
        private const string CLIENT_ID = "804214446242-06p0rtt6f6p0gk8l0k1mrs854jtfv5vr.apps.googleusercontent.com";

        public async Task<GoogleJsonWebSignature.Payload> ValidateGoogleTokenAsync(string idToken)
        {
            try
            {
                var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new[] { CLIENT_ID }
                });

                return payload;
            }
            catch (Exception ex)
            {
                throw new Exception("Invalid Google ID token.", ex);
            }
        }
    }
}
