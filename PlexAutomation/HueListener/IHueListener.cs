using System;
using HueListener.Notification;

namespace HueListener
{
    public interface IHueListener
    {
        void StartListener();

        event EventHandler<HueNotificationEventArgs> OnNewNotification;
    }
}
