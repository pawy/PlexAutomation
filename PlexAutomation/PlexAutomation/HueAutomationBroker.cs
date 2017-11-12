using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using HueListener;
using HueListener.Notification;
using Notificators;
using EventType = HueListener.Notification.EventType;
using PlexListener;
using PlexListener.PMC;
using DenonListener;
using System.Linq;

namespace PlexAutomation
{
    public class HueAutomationBroker : IBroker
    {
        public IHueListener HueListener { get; private set; }

        public IPLexListener PlexListenerService { get; private set; }

        public IDenonListener DenonListenerService { get; set; }

        public List<INotifier> Notifiers { get; private set; }

        public List<IBroker> Brokers { get; private set; }

        public delegate void MessageEventHandler(string message);
        public event MessageEventHandler OnMessage;

        public HueAutomationBroker(IHueListener hueListener, List<INotifier> notifiers, List<IBroker> brokers, IPLexListener plexListener, IDenonListener denonListener)
        {
            Notifiers = notifiers;
            Brokers = brokers;
            HueListener = hueListener;
            PlexListenerService = plexListener;
            DenonListenerService = denonListener;
            HueListener.OnNewNotification += OnNewNotification;
        }

        public void Start()
        {
            HueListener.StartListener();
            SendMessage("Hue Automation Broker started");
        }

        public void Stop()
        {
            HueListener.StartListener();
            SendMessage("Hue Automation Broker stopped");
        }

        private void OnNewNotification(object sender, HueNotificationEventArgs e)
        {
            if (e.HueListenerEventData.EventType == EventType.Error)
            {
                SendMessage(string.Format("Error: {0}", e.HueListenerEventData.ErrorMessage));
                return;
            }

            SendMessage(string.Format("Event: {0}", e.HueListenerEventData.EventType));

            if (PlexListenerService.GetStatus().EventType == PlexListener.Notification.EventType.Playing)
            {
                SendMessage(string.Format("Doing nothing because Plex is playing"));
                return;
            }

            if (new [] {DenonListener.Notification.EventType.SourceCableSat, DenonListener.Notification.EventType.SourceXbox}.Contains(DenonListenerService.GetStatus().EventType))
            {
                SendMessage(string.Format("Doing nothing because Xbox or TV is running"));
                return;
            }

            if (e.HueListenerEventData.EventType == EventType.On)
            {
                Brokers.ForEach(x => x.Start());
                Notify(Notificators.EventType.TurnOn);
            }
            else if(e.HueListenerEventData.EventType == EventType.NotReachable || e.HueListenerEventData.EventType == EventType.Off)
            {
                Brokers.ForEach(x => x.Stop());
                Notify(Notificators.EventType.TurnOff);
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
