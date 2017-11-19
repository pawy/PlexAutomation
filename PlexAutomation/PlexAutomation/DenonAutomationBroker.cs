using System;
using System.Collections.Generic;
using Notificators;
using DenonListener;
using DenonListener.Notification;
using EventType = DenonListener.Notification.EventType;
using PlexListener.PMC;

namespace PlexAutomation
{
    public class DenonAutomationBroker : IBroker
    {
        public IDenonListener DenonListener { get; private set; }

        public List<INotifier> Notifiers { get; private set; }

        public delegate void MessageEventHandler(string message);
        public event MessageEventHandler OnMessage;

        public DenonAutomationBroker(IDenonListener denonListener, List<INotifier> notifiers)
        {
            Notifiers = notifiers;
            DenonListener = denonListener;
            DenonListener.OnNewNotification += OnNewNotification;
        }

        public void Start()
        {   
            DenonListener.StartListener();
            SendMessage("Denon Automation Broker started");
        }

        public void Stop()
        {
            DenonListener.StopListener();
            SendMessage("Denon Automation Broker stopped");
        }

        private void OnNewNotification(object sender, DenonNotificationEventArgs e)
        {
            if (e.DenonListenerEventData.EventType == EventType.Error)
            {
                SendMessage(string.Format("Error: {0}", e.DenonListenerEventData.ErrorMessage));
                return;
            }

            SendMessage(string.Format("Event: {0}",e.DenonListenerEventData.EventType));

            if(PlexListenerService.LastState.EventType == PlexListener.Notification.EventType.Playing && e.DenonListenerEventData.EventType == EventType.SourceBD)
            {
                SendMessage(string.Format("Doing nothing because Plex is Playing"));
                return;
            }

            Notificators.EventType notificationEvent = e.DenonListenerEventData.EventType == EventType.SourceXbox || e.DenonListenerEventData.EventType == EventType.SourceCableSat
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
