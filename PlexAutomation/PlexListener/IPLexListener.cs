using System;
using PlexListener.Notification;

namespace PlexListener
{
    public interface IPLexListener
    {
        void StartListener();

        void StopListener();

        event EventHandler<PlexNotificationEventArgs> OnNewNotification;

        PlexListenerEventData GetStatus();
    }
}
