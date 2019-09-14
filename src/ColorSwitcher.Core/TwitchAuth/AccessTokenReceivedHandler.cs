using MediatR;
using Microsoft.Extensions.Options;
using System.Threading;
using System.Threading.Tasks;

namespace ColorSwitcher.Core.TwitchAuth
{
    public class AccessTokenReceivedHandler : IRequestHandler<AccessTokenReceived, bool>
    {
        private readonly TwitchAuthentication _twitchAuthentication;
        private readonly TwitchSettings _twitchSettings;

        public AccessTokenReceivedHandler(IOptions<TwitchSettings> twitchSettings,
            TwitchAuthentication twitchAuthentication)
        {
            _twitchAuthentication = twitchAuthentication;
            _twitchSettings = twitchSettings.Value;
        }

        public Task<bool> Handle(AccessTokenReceived request,
            CancellationToken cancellationToken)
        {
            bool stateMatches = _twitchAuthentication.States.Contains(request.State);
            if (stateMatches && request.TokenType == "bearer")
            {
                _twitchSettings.AccessToken = $"oauth:{request.AccessToken}";

                // TODO: Save the change.
                // https://stackoverflow.com/questions/40970944/how-to-update-values-into-appsetting-json/42705862

                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }
    }
}