﻿namespace Server.Models.Requests
{
    public class GoogleLoginRequest
    {
        public string IdToken { get; set; }

        public GoogleLoginRequest()
        {
            IdToken = string.Empty;
        }
    }
}
