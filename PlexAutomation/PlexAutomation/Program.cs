using System;
using System.Collections.Generic;
using Notificators;
using Notificators.Hue;
using Notificators.MyStrom;
using PlexListener.PMC;

namespace PlexAutomation
{
    class Program
    {    
        static void Main(string[] args)
        {
            //Create Notifiers
            var notifiers = new List<INotifier>
            {
                new MyStromNotifier("192.168.1.27"),
                new HueNotifier("192.168.1.32", new List<int> {11, 12})
            };

            //Create Listener
            var listener = new PlexListenerService("192.16.8.1.31");

            var plexAutomation = new PlexAutomationBroker(listener, notifiers);
            plexAutomation.OnMessage += Console.WriteLine;
            plexAutomation.Start();

            Console.WriteLine("Plex-Automation-Client running");
            Console.WriteLine("Press <ENTER> to stop the Plex-Automation-Client");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.ReadKey();
        }
    }
}
