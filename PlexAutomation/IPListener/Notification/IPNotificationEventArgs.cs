using System;

namespace IPListener.Notification
{
    public class IPNotificationEventArgs : EventArgs
    {
        public IPNotificationEventArgs(IPListenerEventData eventData)
        {
            IPListenerEventData = eventData;
        }

        public IPListenerEventData IPListenerEventData { get; set; }
    }
}
