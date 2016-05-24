using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using IPListener;
using IPListener.Notification;
using Notificators;
using EventType = IPListener.Notification.EventType;

namespace PlexAutomation
{
    public class IPAutomationBroker : IBroker
    {
        public IIpListener IPListener { get; private set; }

        public List<INotifier> Notifiers { get; private set; }

        public List<IBroker> Brokers { get; private set; }

        public delegate void MessageEventHandler(string message);
        public event MessageEventHandler OnMessage;

        public IPAutomationBroker(IIpListener ipListener, List<INotifier> notifiers)
        {
            Notifiers = notifiers;
            IPListener = ipListener;
            IPListener.OnNewNotification += OnNewNotification;
        }

        public void Start()
        {
            IPListener.StartListener();
            SendMessage("IP Automation Broker started");
        }

        public void Stop()
        {
            IPListener.StartListener();
            SendMessage("IP Automation Broker stopped");
        }

        private void OnNewNotification(object sender, IPNotificationEventArgs e)
        {
            if (e.IPListenerEventData.EventType == EventType.Error)
            {
                SendMessage(string.Format("Error: {0}", e.IPListenerEventData.ErrorMessage));
                return;
            }

            SendMessage(string.Format("Event: {0}",e.IPListenerEventData.EventType));

            if (e.IPListenerEventData.EventType == EventType.On)
            {
                Notify(Notificators.EventType.TurnOff);
            }
            else if(e.IPListenerEventData.EventType == EventType.Off)
            {
                Notify(Notificators.EventType.TurnOn);
            }
        }

        private void Notify(Notificators.EventType notificationEvent)
        {
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
