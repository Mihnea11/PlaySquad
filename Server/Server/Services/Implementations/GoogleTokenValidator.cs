using Google.Apis.Auth;
using Server.Services.Interfaces;

namespace Server.Services.Implementations
{
    public class GoogleTokenValidator : IGoogleTokenValidator
    {
        private const string CLIENT_ID = "804214446242-75de7rupc1qj1c3hvaaedrt4nhdjljke.apps.googleusercontent.com";

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
