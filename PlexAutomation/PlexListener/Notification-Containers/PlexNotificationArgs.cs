using System;

namespace PlexListener
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
