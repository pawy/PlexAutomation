using System;

namespace HueListener.Notification
{
    public class HueNotificationEventArgs : EventArgs
    {
        public HueNotificationEventArgs(HueListenerEventData eventData)
        {
            HueListenerEventData = eventData;
        }

        public HueListenerEventData HueListenerEventData { get; set; }
    }
}
