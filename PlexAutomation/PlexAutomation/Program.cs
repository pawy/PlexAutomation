using System;
using System.Collections.Generic;
using Notificators;
using Notificators.Hue;
using Notificators.MyStrom;
using PlexListener;
using PlexListener.Notification;
using PlexListener.PMC;

namespace PlexAutomation
{
    class Program
    {
        private static IPLexListener _pLexListener;
        private static INotifier _myStromNotifier;
        private static INotifier _hueNotifier;
        
        static void Main(string[] args)
        {
            //PlexListenerConfig listenerConfig = new PlexListenerConfig(new Uri(@"C:\Users\Pascal\Desktop\plex_playing.xml"));
            //listenerConfig.PlayerIps.Add("194.230.159.246");
            //_pLexListener = new PlexListenerService(listenerConfig);

            _pLexListener = new PlexListenerService("192.168.1.31");

            _myStromNotifier = new MyStromNotifier(new Uri("http://192.168.1.27"));
            _hueNotifier = new HueNotifier("192.168.1.32",new List<int>(){11,12});

            _pLexListener.OnNewNotification += OnNewNotification;
            _pLexListener.StartListener();

            Console.WriteLine("Press <ENTER> to exit the Plex-Automation-Client");
            Console.ReadKey();
        }

        private static void OnNewNotification(object sender, PlexNotificationEventArgs e)
        {
            Console.WriteLine("{0}", e.PlexListenerEventData.EventType);

            Notificators.EventType notificationEvent;
            Enum.TryParse(e.PlexListenerEventData.EventType.ToString(), out notificationEvent);

            try
            {
                _myStromNotifier.Notify(notificationEvent);
                Console.WriteLine("Notified MyStrom");
                _hueNotifier.Notify(notificationEvent);
                Console.WriteLine("Notified Hue");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to Notify MyStrom: {0}", ex.Message);
            }
            
        }
    }
}
