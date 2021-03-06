﻿using System;
using System.Linq;
using System.Timers;
using PlexListener.Notification;
using PlexListener.PMC.WebCommunication;
using PlexListener.PMC.XMLContainers;

namespace PlexListener.PMC
{
    public class PlexListenerService : IPLexListener, IDisposable
    {
        public static PlexListenerEventData LastState = new PlexListenerEventData() { EventType = EventType.NotPlaying };

        private readonly PlexWebChecker _plexWebChecker;
        
        private readonly Timer _timer;

        private PlexListenerEventData _lastEventData;

        public PlexListenerService(string clientIp)
        {
            _plexWebChecker = new PlexWebChecker(new Uri(string.Format("http://{0}:3005/player/timeline/poll?commandID=1245", clientIp)));

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

        public event EventHandler<PlexNotificationEventArgs> OnNewNotification;

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
                LastState = eventData;
                Notify(eventData);
            }

        }

        private PlexListenerEventData CreateEventDataFromMediaContainer(MediaContainer mediaContainer)
        {
            if (mediaContainer.Timelines.Any(timeline => timeline.State == "playing"))
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

        public PlexListenerEventData GetStatus()
        {
            PlexListenerEventData eventData = null;
            try
            {
                eventData = CreateEventDataFromMediaContainer(_plexWebChecker.Check());
            }
            catch (Exception ex)
            {
                eventData = new PlexListenerEventData
                {
                    EventType = EventType.Error,
                    ErrorMessage = ex.Message
                };
            }

            return eventData;
        }
    }
}
