using System;
using System.Linq;
using System.Timers;
using DenonListener.Notification;
using DenonListener.WebCommunication;
using DenonListener.XMLContainers;
using System.Net;

namespace DenonListener
{
    public class DenonListenerService : IDenonListener, IDisposable
    {
        private readonly DenonWebChecker _denonWebChecker;
        
        private readonly Timer _timer;

        private DenonListenerEventData _lastEventData;

        public DenonListenerService(string clientIp)
        {
            _denonWebChecker = new DenonWebChecker(new Uri(string.Format("http://{0}/goform/formMainZone_MainZoneXml.xml?_=1509998319336", clientIp)));

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

        public event EventHandler<DenonNotificationEventArgs> OnNewNotification;

        private void Check()
        {
            if (OnNewNotification == null)
            {
                return;
            }

            var eventData = GetStatus();
            if (_lastEventData == null || (eventData != null && _lastEventData.EventType != eventData.EventType))
            {
                _lastEventData = eventData;
                Notify(eventData);
            }
        }


        private DenonListenerEventData CreateEventDataFromItem(Item item)
        {
            var eventData = new DenonListenerEventData();
            switch(item.InputFuncSelectMain.Value)
            {
                case "BD":
                    eventData.EventType = EventType.SourceBD;
                    break;
                case "SAT/CBL":
                    eventData.EventType = EventType.SourceCableSat;
                    break;
                case "GAME":
                    eventData.EventType = EventType.SourceXbox;
                    break;
                default:
                    eventData.EventType = EventType.SourceOther;
                    break;
            }
            return eventData;
        }

        private void Notify(DenonListenerEventData eventData)
        {
            if (OnNewNotification == null)
            {
                return;
            }

            OnNewNotification(this, new DenonNotificationEventArgs(eventData));
        }

        public void Dispose()
        {
            _timer.Stop();
            _timer.Dispose();
        }

        public DenonListenerEventData GetStatus()
        {
            DenonListenerEventData eventData = null;
            try
            {
                eventData = CreateEventDataFromItem(_denonWebChecker.Check());
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ConnectFailure || ex.Status == WebExceptionStatus.Timeout)
                {
                    eventData = new DenonListenerEventData
                    {
                        EventType = EventType.Off
                    };
                }
                else
                {
                    eventData = new DenonListenerEventData
                    {
                        EventType = EventType.Error,
                        ErrorMessage = ex.Message
                    };
                }
            }
            catch (Exception ex)
            {
                eventData = new DenonListenerEventData
                {
                    EventType = EventType.Error,
                    ErrorMessage = ex.Message
                };
            }
            return eventData;
        }
    }
}
