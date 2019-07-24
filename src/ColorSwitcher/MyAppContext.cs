using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using ColorSwitcher.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;

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
                .ConfigureWebHostDefaults(builder => builder.UseStartup<Web.Startup>())
                .ConfigureServices(ConfigureServices)
                .Build();

            _webHost.Start();
        }

        private void ConfigureServices(IServiceCollection services)
        {
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