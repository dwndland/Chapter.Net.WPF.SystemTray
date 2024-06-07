<img src="https://raw.githubusercontent.com/dwndlnd/Chapter.Net.WPF.SystemTray/master/Icon.png" alt="logo" width="64"/>

# Chapter.Net.WPF.SystemTray Library

## Overview
As everybody know, up to now, end of 2024, WPF itself does still not have any build in functionality to maintain a tray icon or windows notification.
Many people therefore go ahead and use the NotifyIcon from System.Windows.Forms. Thats fine in general, to have at least something. But you cannot have special features and you cannot style it properly without struggling with the Forms Themeing.

Chapter.Net.WPF.SystemTray now fills that gap. With that library you have a control to create the tray icon, configure its behavior and you can use the WPF ContextMenu with all its features.
Styling the context menu in the system tray was never more easy. Just as an example.

## Preview
* Unstyled with Native WPF  
![Default](https://raw.githubusercontent.com/dwndlnd/Chapter.Net.WPF.SystemTray/master/Images/Default.png)
* Styled with a light and dark theming  
![Light](https://raw.githubusercontent.com/dwndlnd/Chapter.Net.WPF.SystemTray/master/Images/Light.png) ![Dark](https://raw.githubusercontent.com/dwndlnd/Chapter.Net.WPF.SystemTray/master/Images/Dark.png)
* Special Features  
![Options](https://raw.githubusercontent.com/dwndlnd/Chapter.Net.WPF.SystemTray/master/Images/Options.png)

## Features
- **Tray icon management:** Have a tray icon for a owner window.
- **Context menu customization:** Create, extend and style the context menu shown on the tray icon same ways as for any WPF control.
- **Event handling:** Have access to the tray icon and its context menu by usual bindings on commands, or direct clicks.
- **Minimize to tray:** Allows to configure the owner window to disappear from the task bar when minimized
- **ToolTips:** Allows to set and update the tray icon tool tip at any time.
- **Open window on single or double click:** Have a build in feature to bring the owner window back up by single or double click
- **Icon:** Define the tray icon itself by giving a relative URL to an image or even application.
- **Hide if window shown:** Build in special feature to show the tray icon only if the window is minimized.
- **Notifications:** Show a windows notification, or so called baloon tip, on an easy way.

## Getting Started

1. **Installation:**
    - Install the Chapter.Net.WPF.SystemTray library via NuGet Package Manager:
    ```bash
    dotnet add package Chapter.Net.WPF.SystemTray
    ```

2. **Initialization A:**
    - Initialize the system tray in your WPF application using XAML:
    ```xaml
    <Window>
        <chapter:Tray.TrayIcon>
            <chapter:TrayIcon Icon="ApplicationName.exe" />
        </chapter:Tray.TrayIcon>
    </Window>
    ```

3. **Initialization B:**
    - Initialize the system tray in your WPF application using C#
    ```csharp
    public partial class MainWindow
    {
        private readonly TrayIcon _icon;

        public MainWindow()
        {
            InitializeComponent();

            _icon = new TrayIcon("ApplicationName.exe", this);
            _icon.Show();
        }
    }
    ```

4. **Context menu:**
    - Use the WPF build in ContextMenu control to configure and style the context menu shown on the TrayIcon.
    ```csharp
    <chapter:TrayIcon>
        <chapter:TrayIcon.ContextMenu>
            <ContextMenu>
                <chapter:ShowWindowMenuItem FontSize="{Binding FontSize}" FontWeight="Bold" Header="My Application" />
                <MenuItem Command="{Binding IncreaseFontCommand}" Header="Increase Font Settings" />
                <MenuItem Command="{Binding DecreaseFontCommand}" Header="Decrease Font Settings" />
                <Separator />
                <MenuItem Command="{Binding ShowNotificationCommand}" Header="Show Notification" />
                <Separator />
                <MenuItem Command="{Binding ShutdownCommand}" Header="Quit" />
            </ContextMenu>
        </chapter:TrayIcon.ContextMenu>
    </chapter:TrayIcon>
    ```

5. **Events / Commands:**
    - Next to all what the MenuItem can; the tray icon itself has also commands and click callbacks available.
    ```csharp
    <chapter:TrayIcon ClickCommand="{Binding SingleClickCommand}"
                      ClickCommandParameter="Single Click Parameter"
                      Click="OnSingleClick"
                      DoubleClickCommand="{Binding DoubleClickCommand}"
                      DoubleClickCommandParameter="Double Click Parameter"
                      DoubleClick="OnDoubleClick"/>
    ```

6. **Notifications:**
    - When use bindings and XAML only, simply bind a notification data object and the notification will be shown on any change. Even when its the same data object reference.
    ```XAML
    <chapter:Tray.TrayIcon>
        <chapter:TrayIcon Notification="{Binding Notification}" />
    </chapter:Tray.TrayIcon>
    ```
    ```csharp
    public class MainViewModel : ObservableObject
    {
        private NotificationData _notification;

        public NotificationData Notification
        {
            get => _notification;
            private set => NotifyAndSetIfChanged(ref _notification, value);
        }

        public void ShowNotification()
        {
            Notification = new NotificationData("Caption", "Content", NotificationIcon.Info);
        }
    }
    ```
    - If the TrayIcon is accessible by C# code, the notification can be shown directly:
    ```csharp
    public partial class MainWindow
    {
        private readonly TrayIcon _icon;

        private void ShowNotificationClick(object sender, RoutedEventArgs e)
        {
            _icon.ShowNotification(new NotificationData("Caption", "Content", NotificationIcon.Info));
        }
    }
    ```

7. **Extra features:**
    - Most features are listed in this read me on top. But one more to mention is the build in MenuItem to bring the owner window back up from minimized state without any custom code needed.
    ```XAML
    <chapter:TrayIcon>
        <chapter:TrayIcon.ContextMenu>
            <ContextMenu>
                <chapter:ShowWindowMenuItem FontSize="{Binding FontSize}" FontWeight="Bold" Header="My Application" />
            </ContextMenu>
        </chapter:TrayIcon.ContextMenu>
    </chapter:TrayIcon>
    ```

## Example
- XAML with Bindings
    ```XAML
    <Window>
        <chapter:Tray.TrayIcon>
            <chapter:TrayIcon Icon="MyApplication.exe"
                              Notification="{Binding Notification}"
                              OpenWindowOnClick="True"
                              ToolTip="My Application">
                <chapter:TrayIcon.ContextMenu>
                    <ContextMenu>
                        <chapter:ShowWindowMenuItem FontSize="{Binding FontSize}" FontWeight="Bold" Header="My Application" />
                        <MenuItem Command="{Binding IncreaseFontCommand}" Header="Increase Font Settings" />
                        <MenuItem Command="{Binding DecreaseFontCommand}" Header="Decrease Font Settings" />
                        <Separator />
                        <MenuItem Command="{Binding ShowNotificationCommand}" Header="Show Notification" />
                        <Separator />
                        <MenuItem Command="{Binding ShutdownCommand}" Header="Quit" />
                    </ContextMenu>
                </chapter:TrayIcon.ContextMenu>
            </chapter:TrayIcon>
        </chapter:Tray.TrayIcon>
    </Window>
    ```
    ```csharp
    public class MainViewModel : ObservableObject
    {
        private float _fontSize;
        private NotificationData _notification;

        public MainViewModel()
        {
            FontSize = 12;
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

        public IDelegateCommand IncreaseFontCommand { get; }
        public IDelegateCommand DecreaseFontCommand { get; }
        public IDelegateCommand ShowNotificationCommand { get; }
        public IDelegateCommand ShutdownCommand { get; }

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
    ```
- C# with direct access
    ```csharp
    public partial class MainWindow
    {
        private readonly TrayIcon _icon;
        private readonly MenuItem _mainContextMenuItem;

        public MainWindow()
        {
            InitializeComponent();

            _icon = new TrayIcon("SystemTray_NoPattern.exe", this)
            {
                ToolTip = "My Application",
                ContextMenu = new ContextMenu()
            };
            _icon.ContextMenu.Items.Add(new ShowWindowMenuItem { FontWeight = FontWeights.Bold, Header = "My Application" });
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
    }
    ```

## Links
- [NuGet](https://www.nuget.org/packages/Chapter.Net.WPF.SystemTray)
- [GitHub](https://github.com/dwndlnd/Chapter.Net.WPF.SystemTray)

## License
Copyright (c) David Wendland. All rights reserved.
Licensed under the MIT License. See LICENSE file in the project root for full license information.
