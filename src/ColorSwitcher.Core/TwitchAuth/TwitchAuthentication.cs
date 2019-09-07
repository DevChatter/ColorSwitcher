using System;
using System.Collections.Generic;

namespace ColorSwitcher.Core.TwitchAuth
{
    public class TwitchAuthentication
    {
        public const string ClientId = "lxsrat0rvu5l0cakdcvo44spoo1xh0";
        public List<string> States { get; set; } = new List<string>();

        public const string RedirectUrl = "http://localhost:5234";
        public static string[] Scopes = 
        {
            "chat:edit",
            "chat:read"
        };
        public static string ScopeString = string.Join("+", value: Scopes);


        public string GetUrl()
        {
            string state = Guid.NewGuid().ToString();
            States.Add(state);

            return $"https://id.twitch.tv/oauth2/authorize?client_id={ClientId}&redirect_uri={RedirectUrl}&response_type=token&scope={ScopeString}&state={state}";
        }
    }
}