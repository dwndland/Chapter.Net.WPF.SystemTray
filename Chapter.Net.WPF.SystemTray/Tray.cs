// -----------------------------------------------------------------------------------------------------------------
// <copyright file="Tray.cs" company="dwndland">
//     Copyright (c) David Wendland. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------------------------------------------------

using System;
using System.Windows;

namespace Chapter.Net.WPF.SystemTray;

/// <summary>
///     Brings possibilities for an easy work with the tray icon.
/// </summary>
public sealed class Tray
{
    /// <summary>
    ///     Identifies TrayIcon attached property.
    /// </summary>
    public static readonly DependencyProperty TrayIconProperty =
        DependencyProperty.RegisterAttached("TrayIcon", typeof(TrayIcon), typeof(Tray), new PropertyMetadata(OnTrayIconChanged));

    /// <summary>
    ///     Gets the tray icon to use for the window.
    /// </summary>
    /// <param name="obj">The element from which the property value is read.</param>
    /// <returns>The tray icon.</returns>
    public static TrayIcon GetTrayIcon(DependencyObject obj)
    {
        return (TrayIcon)obj.GetValue(TrayIconProperty);
    }

    /// <summary>
    ///     Sets the tray icon to use for the window.
    /// </summary>
    /// <param name="obj">The element from which the property value is set to.</param>
    /// <param name="value">The tray icon.</param>
    public static void SetTrayIcon(DependencyObject obj, TrayIcon value)
    {
        obj.SetValue(TrayIconProperty, value);
    }

    private static void OnTrayIconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (!(d is Window window))
            throw new InvalidOperationException("The Tray.TrayIcon can be attached to a window only.");

        if (Equals(e.OldValue, e.NewValue))
            return; // No idea why I may come in three or more times in my Example project.

        if (e.OldValue is TrayIcon oldTrayIcon)
            oldTrayIcon.Dispose();
        if (e.NewValue is TrayIcon newTrayIcon)
            newTrayIcon.Initialize(window);
    }
}