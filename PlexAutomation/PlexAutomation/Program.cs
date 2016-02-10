using System;
using System.Collections.Generic;
using HueListener;
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
            Console.WriteLine("Plex-Automation-Client starting...");

            ListenToPlexAndNotifyLights();
            ListenToHueAndNotifyLights();

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Press <ENTER> to stop the Plex-Automation-Client");
            Console.ReadKey();
        }

        /// <summary>
        /// If plex starts playing turn off lights, if it stops turn on lights
        /// </summary>
        private static void ListenToPlexAndNotifyLights()
        {
            //Create Notifiers
            var notifiers = new List<INotifier>
            {
                new MyStromNotifier("192.168.1.27"),
                new HueNotifier("192.168.1.32", new List<int> {11, 12})
            };

            //Create Listener
            var listener = new PlexListenerService("192.168.1.31");

            var plexAutomation = new PlexAutomationBroker(listener, notifiers);
            plexAutomation.OnMessage += message =>
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine(message);
            };
            plexAutomation.Start();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("ListenToPlexAndNotifyLights running...");
        }

        /// <summary>
        /// If hue is turned on, then turn two lamps blue and enable MyStrom
        /// </summary>
        private static void ListenToHueAndNotifyLights()
        {
            //Create Notifiers
            var notifiers = new List<INotifier>
            {
                new MyStromNotifier("192.168.1.27"),
                new HueNotifier("192.168.1.32", new List<int> {11, 12})
            };

            //Create Listener
            var listener = new HueListenerService("192.168.1.32", 11);

            var hueAutomation = new HueAutomationBroker(listener, notifiers);
            hueAutomation.OnMessage += message =>
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(message);
            };
            hueAutomation.Start();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("ListenToHueAndNotifyLights running...");
        }
    }
}
