using System;
using System.Linq;
using System.Timers;
using PlexListener.Notification;
using PlexListener.PMS.WebCommunication;
using PlexListener.PMS.XMLContainers;

namespace PlexListener.PMS
{
    public class PlexListenerService : IPLexListener, IDisposable
    {
        public PlexListenerConfig Configuration { get; private set; }
        private readonly PlexWebChecker _plexWebChecker;
        
        private readonly Timer _timer;

        private PlexListenerEventData _lastEventData;

        public PlexListenerService(PlexListenerConfig configuration)
        {
            _plexWebChecker = new PlexWebChecker(new Uri(string.Format("{0}/status/sessions",configuration.ServerUri)));
            
            Configuration = configuration;
            _timer = new Timer();
            _timer.Interval = 5000;
            _timer.AutoReset = true;
            _timer.Elapsed += OnTimerElapsed;
        }

        public void StartListener()
        {
            _timer.Start();
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            Check();
        }

        public event EventHandler<PlexNotificationEventArgs> OnNewNotification;

        private void Check()
        {
            if (OnNewNotification == null)
            {
                return;
            }

            PlexListenerEventData eventData = CreateEventDataFromMediaContainer(_plexWebChecker.Check());

            if (_lastEventData == null || _lastEventData.EventType != eventData.EventType)
            {
                _lastEventData = eventData;
                Notify(eventData);
            }
        }


        private PlexListenerEventData CreateEventDataFromMediaContainer(MediaContainer mediaContainer)
        {


            if (
                mediaContainer.Photos.Any(photo => !Configuration.PlayerIps.Any() || Configuration.PlayerIps.Contains(photo.Player.Address)) ||
                mediaContainer.Videos.Any(video => video.Player.State == "playing" && (!Configuration.PlayerIps.Any() || Configuration.PlayerIps.Contains(video.Player.Address))))
            {
                return new PlexListenerEventData
                {
                    EventType = EventType.Playing
                };
            }

            return new PlexListenerEventData
            {
                EventType = EventType.NotPlaying
            };
        }

        private void Notify(PlexListenerEventData eventData)
        {
            if (OnNewNotification == null)
            {
                return;
            }

            OnNewNotification(this, new PlexNotificationEventArgs(eventData));
        }

        public void Dispose()
        {
            _timer.Stop();
            _timer.Dispose();
        }
    }
}
