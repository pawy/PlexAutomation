using System;
using HueListener.Notification;
using HueListener.WebCommunication;
using System.Timers;
using HueListener.MessageObjects;

namespace HueListener
{
    public class HueListenerService : IHueListener
    {
        private HueWebChecker _webChecker;
        private readonly Timer _timer;

        public string BridgeIp { get; private set; }
        public int Lamp { get; private set; }

        private HueListenerEventData _lastEventData;

        public HueListenerService(string bridgeIp, int lamp)
        {
            BridgeIp = bridgeIp;
            Lamp = lamp;

            _webChecker = new HueWebChecker(bridgeIp, lamp);

            _timer = new Timer();
            _timer.Interval = 5000;
            _timer.AutoReset = true;
            _timer.Elapsed += OnTimerElapsed;
        }

        public void StartListener()
        {
            _timer.Start();
        }

        public void StopListener()
        {
            _lastEventData = null;
            _timer.Stop();
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            Check();
        }

        public event EventHandler<HueNotificationEventArgs> OnNewNotification;

        private async void Check()
        {
            if (OnNewNotification == null)
            {
                return;
            }

            HueListenerEventData eventData = null;
            try
            {
                eventData = CreateEventDataFromLight(_webChecker.Check());
            }
            catch (Exception ex)
            {
                eventData = new HueListenerEventData
                {
                    EventType = EventType.Error,
                    ErrorMessage = ex.Message
                };
            }
            finally
            {
                if (_lastEventData == null || (eventData != null && _lastEventData.EventType != eventData.EventType))
                {
                    _lastEventData = eventData;
                    Notify(eventData);
                }
            }
        }

        private HueListenerEventData CreateEventDataFromLight(Light light)
        {
            if (!light.State.Reachable)
            {
                return new HueListenerEventData
                {
                    EventType = EventType.NotReachable
                };
            }

            if (light.State.On)
            {
                return new HueListenerEventData
                {
                    EventType = EventType.On
                };
            }

            return new HueListenerEventData
            {
                EventType = EventType.Off
            };
        }

        private void Notify(HueListenerEventData eventData)
        {
            if (OnNewNotification == null)
            {
                return;
            }

            OnNewNotification(this, new HueNotificationEventArgs(eventData));
        }

        public void Dispose()
        {
            _timer.Stop();
            _timer.Dispose();
        }
    }
}
