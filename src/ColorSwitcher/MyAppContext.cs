﻿using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace ColorSwitcher
{
    internal class MyAppContext : ApplicationContext
    {
        private readonly NotifyIcon _notifyIcon;

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
        }

        private void OnExitClick(object sender, EventArgs e)
        {
            _notifyIcon.Visible = false; // TODO: Do this on other types of closing
            Application.Exit();
        }
    }
}