using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using HueListener;
using HueListener.Notification;
using Notificators;
using EventType = HueListener.Notification.EventType;

namespace PlexAutomation
{
    public class HueAutomationBroker : IBroker
    {
        public IHueListener HueListener { get; private set; }

        public List<INotifier> Notifiers { get; private set; }

        public List<IBroker> Brokers { get; private set; }

        public delegate void MessageEventHandler(string message);
        public event MessageEventHandler OnMessage;

        public HueAutomationBroker(IHueListener hueListener, List<INotifier> notifiers, List<IBroker> brokers)
        {
            Notifiers = notifiers;
            Brokers = brokers;
            HueListener = hueListener;
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

            SendMessage(string.Format("Event: {0}",e.HueListenerEventData.EventType));

            //Send a NotPlaying Event when the Hue is turned on -> Hue Go blue, MyStrom goes on too
            if (e.HueListenerEventData.EventType == EventType.On)
            {
                Notify(Notificators.EventType.NotPlaying);
                Brokers.ForEach(x => x.Start());
            }
            else if(e.HueListenerEventData.EventType == EventType.NotReachable)
            {
                Notify(Notificators.EventType.Playing);
                Brokers.ForEach(x => x.Stop());
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
