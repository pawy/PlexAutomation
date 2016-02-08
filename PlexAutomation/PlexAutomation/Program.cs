using System;
using Notificators;
using Notificators.MyStrom;
using PlexListener;
using EventType = Notificators.EventType;

namespace PlexAutomation
{
    class Program
    {
        private static IPLexListener _pLexListener;
        private static INotifier _myStromNotifier;
        
        static void Main(string[] args)
        {
            PlexListenerConfig listenerConfig = new PlexListenerConfig(new Uri(@"C:\Users\Pascal\Desktop\plex_playing.xml"));
            listenerConfig.PlayerIps.Add("194.230.159.246");
            _pLexListener = new PlexListenerService(listenerConfig);

            _myStromNotifier = new MyStromNotifier(new Uri("http://192.168.1.55"));

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
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to Notify MyStrom: {0}", ex.Message);
            }
            
        }
    }
}
