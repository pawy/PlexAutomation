using System;
using System.Collections.Generic;
using Notificators;
using PlexListener;
using PlexListener.Notification;
using EventType = PlexListener.Notification.EventType;

namespace PlexAutomation
{
    public class PlexAutomationBroker
    {
        public IPLexListener PLexListener { get; private set; }

        public List<INotifier> Notifiers { get; private set; }

        public delegate void MessageEventHandler(string message);
        public event MessageEventHandler OnMessage;

        public PlexAutomationBroker(IPLexListener plexListener, List<INotifier> notifiers)
        {
            Notifiers = notifiers;
            PLexListener = plexListener;
        }

        public void Start()
        {
            PLexListener.OnNewNotification += OnNewNotification;
            PLexListener.StartListener();
        }

        private void OnNewNotification(object sender, PlexNotificationEventArgs e)
        {
            if (e.PlexListenerEventData.EventType == EventType.Error)
            {
                SendMessage(string.Format("Error: {0}", e.PlexListenerEventData.ErrorMessage));
                return;
            }

            SendMessage(string.Format("Event: {0}",e.PlexListenerEventData.EventType));

            Notificators.EventType notificationEvent;
            Enum.TryParse(e.PlexListenerEventData.EventType.ToString(), out notificationEvent);

            foreach (INotifier notifier in Notifiers)
            {
                try
                {
                    notifier.Notify(notificationEvent);
                    SendMessage(string.Format("Notified {0}", notifier.GetDisplayName()));
                }
                catch (Exception ex)
                {
                    SendMessage(string.Format("Unable to notify {0}: {1}", notifier.GetDisplayName(), ex.Message));
                }
            }
        }

        private void SendMessage(string message)
        {
            if (OnMessage != null)
            {
                OnMessage(message);
            }
        }
    }
}
