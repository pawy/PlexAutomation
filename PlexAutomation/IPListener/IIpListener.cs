using System;
using IPListener.Notification;

namespace IPListener
{
    public interface IIpListener
    {
        void StartListener();

        void StopListener();

        event EventHandler<IPNotificationEventArgs> OnNewNotification;
    }
}
