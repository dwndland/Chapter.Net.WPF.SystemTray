// 
// Copyright (c) David Wendland. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// 

using System;
using System.Diagnostics;
using Chapter.Net;
using Chapter.Net.WPF.SystemTray;

namespace SystemTray_MVVM.ViewModels;

public class MainViewModel : ObservableObject
{
    private float _fontSize;
    private NotificationData _notification;
    private bool _openWindowOnClick;
    private bool _openWindowOnDoubleClick;
    private bool _minimizeToTray;
    private string _toolTip;
    private bool _hideIfWindowShown;

    public MainViewModel()
    {
        FontSize = 12;
        _openWindowOnDoubleClick = true;
        _toolTip = "Chapter SystemTray";
        SingleClickCommand = new DelegateCommand<string>(SingleClick);
        DoubleClickCommand = new DelegateCommand<string>(DoubleClick);
        IncreaseFontCommand = new DelegateCommand(IncreaseFont);
        DecreaseFontCommand = new DelegateCommand(DecreaseFont);
        ShowNotificationCommand = new DelegateCommand(ShowNotification);
        ShutdownCommand = new DelegateCommand(Shutdown);
    }

    public float FontSize
    {
        get => _fontSize;
        private set => NotifyAndSetIfChanged(ref _fontSize, value);
    }

    public NotificationData Notification
    {
        get => _notification;
        private set => NotifyAndSetIfChanged(ref _notification, value);
    }

    public bool OpenWindowOnClick
    {
        get => _openWindowOnClick;
        set => NotifyAndSetIfChanged(ref _openWindowOnClick, value);
    }

    public bool OpenWindowOnDoubleClick
    {
        get => _openWindowOnDoubleClick;
        set => NotifyAndSetIfChanged(ref _openWindowOnDoubleClick, value);
    }

    public bool MinimizeToTray
    {
        get => _minimizeToTray;
        set => NotifyAndSetIfChanged(ref _minimizeToTray, value);
    }

    public string ToolTip
    {
        get => _toolTip;
        set => NotifyAndSetIfChanged(ref _toolTip, value);
    }

    public bool HideIfWindowShown
    {
        get => _hideIfWindowShown;
        set => NotifyAndSetIfChanged(ref _hideIfWindowShown, value);
    }

    public IDelegateCommand SingleClickCommand { get; }
    public IDelegateCommand DoubleClickCommand { get; }
    public IDelegateCommand IncreaseFontCommand { get; }
    public IDelegateCommand DecreaseFontCommand { get; }
    public IDelegateCommand ShowNotificationCommand { get; }
    public IDelegateCommand ShutdownCommand { get; }

    private void SingleClick(string parameter)
    {
        Debug.WriteLine(parameter);
    }

    private void DoubleClick(string parameter)
    {
        Debug.WriteLine(parameter);
    }

    private void IncreaseFont()
    {
        FontSize += 1;
    }

    private void DecreaseFont()
    {
        FontSize -= 1;
    }

    private void ShowNotification()
    {
        Notification = new NotificationData("Caption", "Content", NotificationIcon.Info);
    }

    private void Shutdown()
    {
        Environment.Exit(0); // Normally I would do that with the Chapter.WPF.Navigation.
    }
}