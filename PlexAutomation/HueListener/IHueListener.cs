using System;
using HueListener.Notification;

namespace HueListener
{
    public interface IHueListener
    {
        void StartListener();

        void StopListener();

        event EventHandler<HueNotificationEventArgs> OnNewNotification;
    }
}
