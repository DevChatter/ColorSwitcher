using System;
using System.Drawing;
using Microsoft.Extensions.Options;
using TwitchLib.Client.Events;
using TwitchLib.Client.Interfaces;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Events;

namespace ColorSwitcher.Core
{
    public class ChatBot
    {
        private readonly ITwitchClient _twitchClient;
        private readonly IColorEmitter _colorEmitter;
        private readonly TwitchSettings _twitchSettings;

        public ChatBot(IOptions<TwitchSettings> twitchSettings,
            ITwitchClient twitchClient, IColorEmitter colorEmitter)
        {
            _twitchClient = twitchClient;
            _colorEmitter = colorEmitter;
            _twitchSettings = twitchSettings.Value;
        }

        public void Start()
        {
            var credentials = new ConnectionCredentials(_twitchSettings.Username, _twitchSettings.AccessToken);
            _twitchClient.Initialize(credentials, _twitchSettings.Channel);
            _twitchClient.Connect();

            _twitchClient.OnConnected += TwitchClient_OnConnected;
            _twitchClient.OnDisconnected += TwitchClient_OnDisconnected;
            _twitchClient.OnMessageReceived += TwitchClient_OnMessageReceived;
            _twitchClient.OnJoinedChannel += TwitchClient_OnJoinedChannel;
        }

        private void TwitchClient_OnJoinedChannel(object sender, OnJoinedChannelArgs e)
        {
            _twitchClient.SendMessage(e.Channel, "Hello from the bot!");
        }

        private void TwitchClient_OnDisconnected(object sender, OnDisconnectedEventArgs e)
        {
        }

        private void TwitchClient_OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            string colorString = e.ChatMessage.Message;
            try
            {
                Color color = ColorTranslator.FromHtml(colorString);
                _colorEmitter.ShowNewColor(ColorTranslator.ToHtml(color));
            }
            catch
            {
                // Do nothing in invalid color
            }
        }

        private void TwitchClient_OnConnected(object sender, OnConnectedArgs e)
        {
        }
    }
}