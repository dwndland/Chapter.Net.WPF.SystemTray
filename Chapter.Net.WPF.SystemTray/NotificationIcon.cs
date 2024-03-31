// -----------------------------------------------------------------------------------------------------------------
// <copyright file="NotificationIcon.cs" company="my-libraries">
//     Copyright (c) David Wendland. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------------------------------------------------

namespace Chapter.Net.WPF.SystemTray
{
    /// <summary>
    ///     Represents the icon shown in the notification.
    /// </summary>
    public enum NotificationIcon
    {
        /// <summary>
        ///     No Icon.
        /// </summary>
        None = 0,

        /// <summary>
        ///     A Information Icon.
        /// </summary>
        Info = 1,

        /// <summary>
        ///     A Warning Icon.
        /// </summary>
        Warning = 2,

        /// <summary>
        ///     A Error Icon.
        /// </summary>
        Error = 3
    }
}