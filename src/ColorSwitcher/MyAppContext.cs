using ColorSwitcher.Core;
using ColorSwitcher.Extensions;
using ColorSwitcher.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.Extensions.Configuration;

namespace ColorSwitcher
{
    internal class MyAppContext : ApplicationContext
    {
        private readonly NotifyIcon _notifyIcon;
        private readonly IHost _webHost;

        public MyAppContext()
        {
            var exitMenuItem = new MenuItem("Exit", OnExitClick);

            using Stream iconStream = Assembly.GetExecutingAssembly()
                .GetManifestResourceStream("ColorSwitcher.devchatterlogo.ico");

            var icon = new Icon(iconStream);
            _notifyIcon = new NotifyIcon
            {
                Icon = icon,
                ContextMenu = new ContextMenu(new []{ exitMenuItem }),
                Visible = true
            };

            _webHost = Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(
                    hostBuilder =>
                    {
                        hostBuilder.UseStartup<Startup>();
                    })
                .ConfigureAppConfiguration(configBuilder => configBuilder
                        .AddJsonFile("appsettings.json",
                            optional: false,
                            reloadOnChange: true)
                        .AddEnvironmentVariables()
                        .AddUserSecrets<MyAppContext>())
                .Build();

            var chatBot = _webHost.Services.GetService<ChatBot>();

            chatBot.Start();

            _webHost.Start();
        }

        private void OnExitClick(object sender, EventArgs e)
        {
            _notifyIcon.Visible = false; // TODO: Do this on other types of closing

            // TODO: Get this to close
            _webHost.Services.GetService<IHostApplicationLifetime>().StopApplication();
            _webHost.StopAsync().RunInBackgroundSafely(HandleException);
            Application.Exit();
        }

        private void HandleException(Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
}