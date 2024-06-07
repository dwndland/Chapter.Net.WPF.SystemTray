// -----------------------------------------------------------------------------------------------------------------
// <copyright file="NotificationData.cs" company="my-libraries">
//     Copyright (c) David Wendland. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------------------------------------------------

namespace Chapter.Net.WPF.SystemTray;

/// <summary>
///     Contains the data to show in the notification.
/// </summary>
public sealed class NotificationData
{
    /// <summary>
    ///     Creates a new NotificationData.
    /// </summary>
    /// <param name="content">The notification content.</param>
    public NotificationData(string content)
        : this(null, content, NotificationIcon.None)
    {
    }

    /// <summary>
    ///     Creates a new NotificationData.
    /// </summary>
    /// <param name="caption">The notification caption.</param>
    /// <param name="content">The notification content.</param>
    public NotificationData(string caption, string content)
        : this(caption, content, NotificationIcon.None)
    {
    }

    /// <summary>
    ///     Creates a new NotificationData.
    /// </summary>
    /// <param name="content">The notification content.</param>
    /// <param name="icon">The notification icon.</param>
    public NotificationData(string content, NotificationIcon icon)
        : this(null, content, icon)
    {
    }

    /// <summary>
    ///     Creates a new NotificationData.
    /// </summary>
    /// <param name="caption">The notification caption.</param>
    /// <param name="content">The notification content.</param>
    /// <param name="icon">The notification icon.</param>
    public NotificationData(string caption, string content, NotificationIcon icon)
    {
        Caption = caption;
        Content = content;
        Icon = icon;
    }

    /// <summary>
    ///     Gets or sets the notification caption.
    /// </summary>
    public string Caption { get; set; }

    /// <summary>
    ///     Gets or sets the notification content.
    /// </summary>
    public string Content { get; set; }

    /// <summary>
    ///     Gets or sets the notification icon.
    /// </summary>
    public NotificationIcon Icon { get; set; }
}