using System;

namespace PlexListener.Notification
{
    public class PlexNotificationEventArgs : EventArgs
    {
        public PlexNotificationEventArgs(PlexListenerEventData eventData)
        {
            PlexListenerEventData = eventData;
        }

        public PlexListenerEventData PlexListenerEventData { get; set; }
    }
}
