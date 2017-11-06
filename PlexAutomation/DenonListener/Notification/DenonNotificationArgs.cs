using System;

namespace DenonListener.Notification
{
    public class DenonNotificationEventArgs : EventArgs
    {
        public DenonNotificationEventArgs(DenonListenerEventData eventData)
        {
            DenonListenerEventData = eventData;
        }

        public DenonListenerEventData DenonListenerEventData { get; set; }
    }
}
