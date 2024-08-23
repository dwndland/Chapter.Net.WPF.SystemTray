// -----------------------------------------------------------------------------------------------------------------
// <copyright file="MainView.xaml.cs" company="dwndland">
//     Copyright (c) David Wendland. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------------------------------------------------

using SystemTray_MVVM.ViewModels;

namespace SystemTray_MVVM.Views;

public partial class MainView
{
    public MainView()
    {
        InitializeComponent();

        DataContext = new MainViewModel(); // Normally I would do that with the Chapter.WPF.Navigation.
    }
}