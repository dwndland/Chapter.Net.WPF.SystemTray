// -----------------------------------------------------------------------------------------------------------------
// <copyright file="ShowWindowMenuItem.cs" company="my-libraries">
//     Copyright (c) David Wendland. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------------------------------------------------

using System;
using System.Windows;
using System.Windows.Controls;

namespace Chapter.Net.WPF.SystemTray
{
    /// <summary>
    ///     Represents a menu item to bring a minimized window back to normal or maximized.
    /// </summary>
    public sealed class ShowWindowMenuItem : MenuItem
    {
        /// <summary>
        ///     Creates a new menu item bringing up the given window.
        /// </summary>
        public ShowWindowMenuItem()
        {
            Click += OnClick;
        }

        private void OnClick(object sender, EventArgs e)
        {
            var window = Window.GetWindow(this);
            if (window != null)
            {
                window.ShowInTaskbar = true;
                TrayIcon.RestoreFromMinimize(window);
                window.Activate();
            }
        }
    }
}