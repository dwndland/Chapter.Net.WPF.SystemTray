// -----------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="dwndland">
//     Copyright (c) David Wendland. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------------------------------------------------

// ReSharper disable PossibleNullReferenceException
// ReSharper disable PossibleInvalidOperationException

using System.Windows;
using System.Windows.Controls;
using Chapter.Net.WPF.SystemTray;

namespace Demo_NoPattern;

public partial class MainWindow
{
    private readonly TrayIcon _icon;
    private readonly MenuItem _mainContextMenuItem;

    public MainWindow()
    {
        InitializeComponent();

        _icon = new TrayIcon("Demo_NoPattern.exe", this)
        {
            OpenWindowOnDoubleClick = true,
            ToolTip = "Chapter SystemTray",
            ContextMenu = new ContextMenu()
        };
        _icon.ContextMenu.Items.Add(new ShowWindowMenuItem { FontWeight = FontWeights.Bold, Header = "Chapter SystemTray" });
        _icon.ContextMenu.Items.Add(new MenuItem { Header = "Increase Font Settings" });
        _icon.ContextMenu.Items.Add(new MenuItem { Header = "Decrease Font Settings" });
        _icon.ContextMenu.Items.Add(new Separator());
        _icon.ContextMenu.Items.Add(new MenuItem { Header = "Show Notification" });
        _icon.ContextMenu.Items.Add(new Separator());
        _icon.ContextMenu.Items.Add(new MenuItem { Header = "Quit" });

        _mainContextMenuItem = (MenuItem)_icon.ContextMenu.Items[0];
        ((MenuItem)_icon.ContextMenu.Items[1]).Click += IncreaseFontClick;
        ((MenuItem)_icon.ContextMenu.Items[2]).Click += DecreaseFontClick;
        ((MenuItem)_icon.ContextMenu.Items[4]).Click += ShowNotificationClick;
        ((MenuItem)_icon.ContextMenu.Items[6]).Click += ShutdownClick;

        _icon.Show();
    }

    private void IncreaseFontClick(object sender, RoutedEventArgs e)
    {
        _mainContextMenuItem.FontSize += 1;
    }

    private void DecreaseFontClick(object sender, RoutedEventArgs e)
    {
        _mainContextMenuItem.FontSize -= 1;
    }

    private void ShowNotificationClick(object sender, RoutedEventArgs e)
    {
        _icon.ShowNotification(new NotificationData("Caption", "Content", NotificationIcon.Info));
    }

    private void ShutdownClick(object sender, RoutedEventArgs e)
    {
        _icon.Dispose();
        Close();
    }

    private void OnOpenWindowOnClickClick(object sender, RoutedEventArgs e)
    {
        _icon.OpenWindowOnClick = openWindowOnClick.IsChecked.Value;
    }

    private void OnOpenWindowOnDoubleClickClick(object sender, RoutedEventArgs e)
    {
        _icon.OpenWindowOnDoubleClick = openWindowOnDoubleClick.IsChecked.Value;
    }

    private void OnMinimizeToTrayClick(object sender, RoutedEventArgs e)
    {
        _icon.MinimizeToTray = minimizeToTray.IsChecked.Value;
    }

    private void OnToolTipChanged(object sender, TextChangedEventArgs e)
    {
        if (_icon != null)
            _icon.ToolTip = toolTip.Text;
    }

    private void OnHideIfWindowShownClick(object sender, RoutedEventArgs e)
    {
        _icon.HideIfWindowShown = hideIfWindowShown.IsChecked.Value;
    }
}