// -----------------------------------------------------------------------------------------------------------------
// <copyright file="TrayIcon.cs" company="my-libraries">
//     Copyright (c) David Wendland. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using Chapter.Net.WinAPI;
using ContextMenu = System.Windows.Controls.ContextMenu;

namespace Chapter.Net.WPF.SystemTray;

/// <summary>
///     Represents the application icon in the tray menu.
/// </summary>
public class TrayIcon : Freezable, IDisposable
{
    /// <summary>
    ///     Identifies the ClickCommand property.
    /// </summary>
    public static readonly DependencyProperty ClickCommandProperty =
        DependencyProperty.Register(nameof(ClickCommand), typeof(ICommand), typeof(TrayIcon), new PropertyMetadata(null));

    /// <summary>
    ///     Identifies the ClickCommandParameter property.
    /// </summary>
    public static readonly DependencyProperty ClickCommandParameterProperty =
        DependencyProperty.Register(nameof(ClickCommandParameter), typeof(object), typeof(TrayIcon), new PropertyMetadata(null));

    /// <summary>
    ///     Identifies the OpenWindowOnClick property.
    /// </summary>
    public static readonly DependencyProperty OpenWindowOnClickProperty =
        DependencyProperty.Register(nameof(OpenWindowOnClick), typeof(bool), typeof(TrayIcon), new PropertyMetadata(false));

    /// <summary>
    ///     Identifies the ContextMenu property.
    /// </summary>
    public static readonly DependencyProperty ContextMenuProperty =
        DependencyProperty.Register(nameof(ContextMenu), typeof(ContextMenu), typeof(TrayIcon), new PropertyMetadata(null));

    /// <summary>
    ///     Identifies the DoubleClickCommand property.
    /// </summary>
    public static readonly DependencyProperty DoubleClickCommandProperty =
        DependencyProperty.Register(nameof(DoubleClickCommand), typeof(ICommand), typeof(TrayIcon), new PropertyMetadata(null));

    /// <summary>
    ///     Identifies the DoubleClickCommandParameter property.
    /// </summary>
    public static readonly DependencyProperty DoubleClickCommandParameterProperty =
        DependencyProperty.Register(nameof(DoubleClickCommandParameter), typeof(object), typeof(TrayIcon), new PropertyMetadata(null));

    /// <summary>
    ///     Identifies the OpenWindowOnDoubleClick property.
    /// </summary>
    public static readonly DependencyProperty OpenWindowOnDoubleClickProperty =
        DependencyProperty.Register(nameof(OpenWindowOnDoubleClick), typeof(bool), typeof(TrayIcon), new PropertyMetadata(false));

    /// <summary>
    ///     Identifies the Icon property.
    /// </summary>
    public static readonly DependencyProperty IconProperty =
        DependencyProperty.Register(nameof(Icon), typeof(string), typeof(TrayIcon), new PropertyMetadata(null, OnIconChanged));

    /// <summary>
    ///     Identifies the MinimizeToTray property.
    /// </summary>
    public static readonly DependencyProperty MinimizeToTrayProperty =
        DependencyProperty.Register(nameof(MinimizeToTray), typeof(bool), typeof(TrayIcon), new PropertyMetadata(false));

    /// <summary>
    ///     Identifies the ToolTip property.
    /// </summary>
    public static readonly DependencyProperty ToolTipProperty =
        DependencyProperty.Register(nameof(ToolTip), typeof(string), typeof(TrayIcon), new PropertyMetadata(null, OnToolTipChanged));

    /// <summary>
    ///     Identifies the HideIfWindowShown property.
    /// </summary>
    public static readonly DependencyProperty HideIfWindowShownProperty =
        DependencyProperty.Register(nameof(HideIfWindowShown), typeof(bool), typeof(TrayIcon), new PropertyMetadata(false));

    /// <summary>
    ///     Identifies the Notification property.
    /// </summary>
    public static readonly DependencyProperty NotificationProperty =
        DependencyProperty.Register(nameof(Notification), typeof(NotificationData), typeof(TrayIcon), new PropertyMetadata(null, (o, e) => { }, OnNotificationChanged));

    /// <summary>
    ///     Identifies the TrackClickInside property.
    /// </summary>
    public static readonly DependencyProperty TrackClickInsideProperty =
        DependencyProperty.Register(nameof(TrackClickInside), typeof(bool), typeof(TrayIcon), new PropertyMetadata(false));

    private readonly NotifyIcon _icon;
    private readonly InputWatcher _inputWatcher;
    private Window _window;

    /// <summary>
    ///     Initializes a new instance of the <see cref="TrayIcon" /> class.
    /// </summary>
    public TrayIcon()
    {
        _icon = new NotifyIcon();
        _icon.MouseClick += OnIconClick;
        _icon.MouseDoubleClick += OnIconDoubleClick;
        _inputWatcher = new InputWatcher();
        _inputWatcher.Observe(new MouseInput(MouseAction.LeftClick, OnMouseGlobalClick));
        _inputWatcher.Observe(new MouseInput(MouseAction.MiddleClick, OnMouseGlobalClick));
        _inputWatcher.Observe(new MouseInput(MouseAction.WheelClick, OnMouseGlobalClick));
        _inputWatcher.Observe(new MouseInput(MouseAction.RightClick, OnMouseGlobalClick));
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="TrayIcon" /> class.
    /// </summary>
    /// <param name="icon">The path to the tray icon.</param>
    /// <param name="window">The window the tray icon belongs to.</param>
    public TrayIcon(string icon, Window window)
        : this()
    {
        Icon = icon;
        Initialize(window);
    }

    /// <summary>
    ///     Gets or sets the command to be executed if the user clicks the tray icon.
    /// </summary>
    /// <value>Default: null.</value>
    [DefaultValue(null)]
    public ICommand ClickCommand
    {
        get => (ICommand)GetValue(ClickCommandProperty);
        set => SetValue(ClickCommandProperty, value);
    }

    /// <summary>
    ///     Gets or sets the parameter to be passed with the <see cref="Command" />.
    /// </summary>
    /// <value>Default: null.</value>
    [DefaultValue(null)]
    public object ClickCommandParameter
    {
        get => GetValue(ClickCommandParameterProperty);
        set => SetValue(ClickCommandParameterProperty, value);
    }

    /// <summary>
    ///     Gets or sets the indicator defining if the minimized main window shall bring back up on single click on the tray
    ///     icon.
    /// </summary>
    /// <value>Default: false.</value>
    [DefaultValue(false)]
    public bool OpenWindowOnClick
    {
        get => (bool)GetValue(OpenWindowOnClickProperty);
        set => SetValue(OpenWindowOnClickProperty, value);
    }

    /// <summary>
    ///     Gets or sets the context menu items of the tray icon.
    /// </summary>
    /// <value>Default: null.</value>
    [DefaultValue(null)]
    public ContextMenu ContextMenu
    {
        get => (ContextMenu)GetValue(ContextMenuProperty);
        set => SetValue(ContextMenuProperty, value);
    }

    /// <summary>
    ///     Gets or sets the command to be executed if the user double clicks the tray icon.
    /// </summary>
    /// <value>Default: null.</value>
    [DefaultValue(null)]
    public ICommand DoubleClickCommand
    {
        get => (ICommand)GetValue(DoubleClickCommandProperty);
        set => SetValue(DoubleClickCommandProperty, value);
    }

    /// <summary>
    ///     Gets or sets the parameter to be passed with the <see cref="DoubleClickCommand" />.
    /// </summary>
    /// <value>Default: null.</value>
    [DefaultValue(null)]
    public object DoubleClickCommandParameter
    {
        get => GetValue(DoubleClickCommandParameterProperty);
        set => SetValue(DoubleClickCommandParameterProperty, value);
    }

    /// <summary>
    ///     Gets or sets the indicator defining if the minimized main window shall bring back up on double click on the tray
    ///     icon.
    /// </summary>
    /// <value>Default: false.</value>
    [DefaultValue(false)]
    public bool OpenWindowOnDoubleClick
    {
        get => (bool)GetValue(OpenWindowOnDoubleClickProperty);
        set => SetValue(OpenWindowOnDoubleClickProperty, value);
    }

    /// <summary>
    ///     Gets or sets the path to the tray icon.
    ///     If the file does not exist, the Tray tries to find it by the file name in the application root.
    /// </summary>
    /// <value>Default: null.</value>
    [DefaultValue(null)]
    public string Icon
    {
        get => (string)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    /// <summary>
    ///     Gets or set an indicator if the owner window shall minimize to the system tray or not.
    /// </summary>
    /// <value>Default: false.</value>
    [DefaultValue(false)]
    public bool MinimizeToTray
    {
        get => (bool)GetValue(MinimizeToTrayProperty);
        set => SetValue(MinimizeToTrayProperty, value);
    }

    /// <summary>
    ///     Gets or sets the tooltip shown on the tray icon.
    /// </summary>
    /// <value>Default: null.</value>
    [DefaultValue(null)]
    public string ToolTip
    {
        get => (string)GetValue(ToolTipProperty);
        set => SetValue(ToolTipProperty, value);
    }

    /// <summary>
    ///     Gets or sets the indicator if the tray icon shall be shown only if window is minimized.
    /// </summary>
    /// <value>Default: false.</value>
    [DefaultValue(false)]
    public bool HideIfWindowShown
    {
        get => (bool)GetValue(HideIfWindowShownProperty);
        set => SetValue(HideIfWindowShownProperty, value);
    }

    /// <summary>
    ///     Gets or sets the data what notification to show.
    /// </summary>
    /// <value>Default: null.</value>
    [DefaultValue(null)]
    public NotificationData Notification
    {
        get => (NotificationData)GetValue(NotificationProperty);
        set => SetValue(NotificationProperty, value);
    }

    /// <summary>
    ///     Tries to detect if the click was inside the context menu or not to decide when to close.
    /// </summary>
    /// <remarks>
    ///     This is only needed for special cases when the context menu has to stay open after click inside. It can happen
    ///     with this option on, that under some circumstances the context menu stays open unexpectedly, especially when the
    ///     user clicks outside right from the menu or when click into a text editor in the background right from the context
    ///     menu. If you have a better idea how to track the auto close of the context menu, feel free to inherit the TrayIcon
    ///     and overload the <see cref="WasClickInside" />.
    ///     By default this option is off; so any click anywhere, also inside the context menu, will automatically close the
    ///     context menu.
    /// </remarks>
    /// <value>Default: false.</value>
    [DefaultValue(false)]
    public bool TrackClickInside
    {
        get => (bool)GetValue(TrackClickInsideProperty);
        set => SetValue(TrackClickInsideProperty, value);
    }

    /// <summary>
    ///     Disposes the tray icon.
    /// </summary>
    public void Dispose()
    {
        Hide();
        _icon.Dispose();
        if (_window != null)
        {
            _window.Closed -= OnWindowClosed;
            _window.StateChanged -= OnWindowStateChanged;
        }
    }

    private static object OnNotificationChanged(DependencyObject d, object basevalue)
    {
        if (basevalue is NotificationData data)
        {
            var control = (TrayIcon)d;
            control.ShowNotification(data);
        }

        return basevalue;
    }

    /// <summary>
    ///     Raised if the user single clicks the tray icon.
    /// </summary>
    public event EventHandler Click;

    /// <summary>
    ///     Raised if the user double clicks the tray icon.
    /// </summary>
    public event EventHandler DoubleClick;

    /// <summary>
    ///     Shows the tray icon.
    /// </summary>
    public void Show()
    {
        _icon.Visible = true;
    }

    /// <summary>
    ///     Hides the tray icon.
    /// </summary>
    public void Hide()
    {
        _icon.Visible = false;
    }

    /// <summary>
    ///     Shows a windows notification.
    /// </summary>
    /// <param name="data">The caption of the notification.</param>
    /// <exception cref="ArgumentNullException">The data cannot be null.</exception>
    public void ShowNotification(NotificationData data)
    {
        if (data == null)
            throw new ArgumentNullException(nameof(data));

        _icon.ShowBalloonTip(0, data.Caption, data.Content, (ToolTipIcon)data.Icon);
    }

    /// <summary>
    ///     Sets the window the tray icon belongs to.
    /// </summary>
    /// <param name="window">The owner window.</param>
    /// <exception cref="ArgumentNullException">The window cannot be null.</exception>
    public void Initialize(Window window)
    {
        _window = window ?? throw new ArgumentNullException(nameof(window));
        _window.Closed += OnWindowClosed;
        _window.StateChanged += OnWindowStateChanged;
        _icon.Text = ToolTip;

        if (!HideIfWindowShown || _window.WindowState is WindowState.Minimized)
            Show();
    }

    /// <inheritdoc />
    protected override Freezable CreateInstanceCore()
    {
        return this;
    }

    /// <summary>
    ///     Detects if the user clicked inside the context menu.
    /// </summary>
    /// <returns>True if the user clicked inside; otherwise false.</returns>
    protected virtual bool WasClickInside()
    {
        var position = Mouse.GetPosition(ContextMenu);
        var b = position.X < 0 ||
                position.Y < 0 ||
                position.X >= ContextMenu.ActualWidth || // If more right, the position.X will stick on the same size like the context menu.
                position.Y >= ContextMenu.ActualHeight;
        return b;
    }

    /// <summary>
    ///     Restores the given window out of minimize state to its previous.
    /// </summary>
    /// <param name="window">The window to restore.</param>
    internal static void RestoreFromMinimize(Window window)
    {
        var windowHandle = ((HwndSource)PresentationSource.FromVisual(window)!).Handle;
        User32.ShowWindow(windowHandle, WindowShowStyle.Restore);
    }

    private void OnWindowStateChanged(object sender, EventArgs e)
    {
        if (_window.WindowState != WindowState.Minimized)
        {
            if (HideIfWindowShown)
                Hide();
        }
        else
        {
            Show();
            _window.ShowInTaskbar = !MinimizeToTray;
        }
    }

    private static void OnIconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (e.NewValue == null)
            return;

        var control = (TrayIcon)d;
        if (!File.Exists(control.Icon))
            return;
        control._icon.Icon = Path.GetExtension(control.Icon) == ".exe" ? System.Drawing.Icon.ExtractAssociatedIcon(control.Icon) : new Icon(control.Icon);
    }

    private static void OnToolTipChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var control = (TrayIcon)d;
        control._icon.Text = control.ToolTip;
    }

    private void OnWindowClosed(object sender, EventArgs e)
    {
        Dispose();
    }

    private void OnIconClick(object sender, System.Windows.Forms.MouseEventArgs e)
    {
        if (ContextMenu == null || e.Button != MouseButtons.Right)
        {
            HandleClick();
            return;
        }

        if (e.Button == MouseButtons.Right && !ContextMenu.IsOpen)
            OpenContextMenu();
    }

    private void OnIconDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left || ContextMenu == null) HandleDoubleClick();
    }

    private void HandleClick()
    {
        if (ClickCommand != null && ClickCommand.CanExecute(ClickCommandParameter))
            ClickCommand.Execute(ClickCommandParameter);
        Click?.Invoke(this, EventArgs.Empty);
        if (OpenWindowOnClick)
        {
            _window.ShowInTaskbar = true;
            RestoreFromMinimize(_window);
            _window.Activate();
        }
    }

    private void HandleDoubleClick()
    {
        if (DoubleClickCommand != null && DoubleClickCommand.CanExecute(DoubleClickCommandParameter))
            DoubleClickCommand.Execute(DoubleClickCommandParameter);
        DoubleClick?.Invoke(this, EventArgs.Empty);
        if (OpenWindowOnDoubleClick)
        {
            RestoreFromMinimize(_window);
            _window.Activate();
        }
    }

    private void OnMouseGlobalClick(MouseEventArgs obj)
    {
        if (!TrackClickInside || WasClickInside())
            CloseContextMenu();
    }

    private void CloseContextMenu()
    {
        if (ContextMenu != null)
        {
            ContextMenu.IsOpen = false;
            _inputWatcher.Stop();
        }
    }

    private void OpenContextMenu()
    {
        var contextHost = new Border
        {
            Visibility = Visibility.Collapsed,
            ContextMenu = ContextMenu
        };
        ContextMenu.PlacementTarget = _window;
        contextHost.ContextMenu.IsOpen = true;
        _inputWatcher.Start();
    }
}