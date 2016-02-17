using System;
using System.Collections.Generic;
using Notificators;
using PlexListener;
using PlexListener.Notification;
using EventType = PlexListener.Notification.EventType;

namespace PlexAutomation
{
    public class PlexAutomationBroker : IBroker
    {
        public IPLexListener PLexListener { get; private set; }

        public List<INotifier> Notifiers { get; private set; }

        public delegate void MessageEventHandler(string message);
        public event MessageEventHandler OnMessage;

        public PlexAutomationBroker(IPLexListener plexListener, List<INotifier> notifiers)
        {
            Notifiers = notifiers;
            PLexListener = plexListener;
            PLexListener.OnNewNotification += OnNewNotification;
        }

        public void Start()
        {   
            PLexListener.StartListener();
            SendMessage("Plex Automation Broker started");
        }

        public void Stop()
        {
            PLexListener.StopListener();
            SendMessage("Plex Automation Broker stopped");
        }

        private void OnNewNotification(object sender, PlexNotificationEventArgs e)
        {
            if (e.PlexListenerEventData.EventType == EventType.Error)
            {
                SendMessage(string.Format("Error: {0}", e.PlexListenerEventData.ErrorMessage));
                return;
            }

            SendMessage(string.Format("Event: {0}",e.PlexListenerEventData.EventType));

            Notificators.EventType notificationEvent = e.PlexListenerEventData.EventType == EventType.Playing
                ? Notificators.EventType.TurnOff
                : Notificators.EventType.TurnOn;

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
