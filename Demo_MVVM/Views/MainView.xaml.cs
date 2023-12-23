// 
// Copyright (c) David Wendland. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// 

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