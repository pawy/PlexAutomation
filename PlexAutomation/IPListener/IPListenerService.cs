using System;
using IPListener.Notification;
using IPListener.Communication;
using System.Timers;

namespace IPListener
{
    public class IPListenerService : IIpListener
    {
        private IPChecker _ipChecker;
        private readonly Timer _timer;

        public string Ip { get; private set; } 

        private IPListenerEventData _lastEventData;

        public IPListenerService(string ip)
        {
            Ip = ip;

            _ipChecker = new IPChecker(ip);

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

        public event EventHandler<IPNotificationEventArgs> OnNewNotification;

        private async void Check()
        {
            if (OnNewNotification == null)
            {
                return;
            }

            IPListenerEventData eventData = null;
            try
            {
                var eventType = _ipChecker.Check() ?
                    EventType.On : EventType.Off;

                eventData = new IPListenerEventData
                {
                    EventType = eventType
                };
            }
            catch (Exception ex)
            {
                eventData = new IPListenerEventData
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


        private void Notify(IPListenerEventData eventData)
        {
            if (OnNewNotification == null)
            {
                return;
            }

            OnNewNotification(this, new IPNotificationEventArgs(eventData));
        }

        public void Dispose()
        {
            _timer.Stop();
            _timer.Dispose();
        }
    }
}
