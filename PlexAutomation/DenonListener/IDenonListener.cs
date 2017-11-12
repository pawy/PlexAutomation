using System;
using DenonListener.Notification;

namespace DenonListener
{
    public interface IDenonListener
    {
        void StartListener();

        void StopListener();

        event EventHandler<DenonNotificationEventArgs> OnNewNotification;

        DenonListenerEventData GetStatus();
    }
}
