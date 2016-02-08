using System;
using PlexListener.Notification;

namespace PlexListener
{
    public interface IPLexListener
    {
        void StartListener();

        event EventHandler<PlexNotificationEventArgs> OnNewNotification;
    }
}
