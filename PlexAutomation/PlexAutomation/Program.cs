using System;
using System.Collections.Generic;
using HueListener;
using Notificators;
using Notificators.Hue;
using Notificators.MyStrom;
using PlexListener.PMC;
using IPListener;

namespace PlexAutomation
{
    class Program
    {
        private static PlexAutomationBroker _plexAutomationBroker;
        private static HueAutomationBroker _huerHueAutomationBroker;
        private static IPAutomationBroker _ipAutomationBroker;

        private const ConsoleColor PlexColor = ConsoleColor.DarkGreen;
        private const ConsoleColor HueColor = ConsoleColor.Yellow;
        private const ConsoleColor IPColor = ConsoleColor.Cyan;

        static void Main(string[] args)
        {
            Console.WriteLine("Plex-Automation-Client starting...");

            PlexAutomationBroker plexAutomation = InitializePlexListener(PlexColor);
            HueAutomationBroker hueAutomation = InitializeHueListner(HueColor);
            IPAutomationBroker ipAutomation = InitializeIPListener(IPColor);

            //hueAutomation.Start();
            plexAutomation.Start();
            ipAutomation.Start();

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Press <ENTER> to stop the Plex-Automation-Client");
            Console.ReadKey();
        }

        /// <summary>
        /// If plex starts playing turn off lights
        /// If plex stops playing turn on lights
        /// </summary>
        private static PlexAutomationBroker InitializePlexListener(ConsoleColor consoleColor)
        {
            var notifiers = new List<INotifier>
            {
                new HueNotifier("192.168.1.32", new List<int> {11}),
                new MyStromNotifier("192.168.1.27")
            };

            var listener = new PlexListenerService("192.168.1.34");

            var plexAutomation = new PlexAutomationBroker(listener, notifiers);
            plexAutomation.OnMessage += message => OutputMessage(message, consoleColor);

            return plexAutomation;
        }

        /// <summary>
        /// If TV is turned on, turn off lights
        /// If TV is turned off, turn on lights
        /// </summary>
        private static IPAutomationBroker InitializeIPListener(ConsoleColor consoleColor)
        {
            var notifiers = new List<INotifier>
            {
                new HueNotifier("192.168.1.32", new List<int> {11}),
                new MyStromNotifier("192.168.1.27")
            };

            var listener = new IPListenerService("192.168.1.25");

            var ipAutomation = new IPAutomationBroker(listener, notifiers);
            ipAutomation.OnMessage += message => OutputMessage(message, consoleColor);

            return ipAutomation;
        }


        /// <summary>
        /// If hue is turned on, then turn on the PlexListener
        /// If hue is turned off, then turn off MyStrom and PlexListener
        /// </summary>
        private static HueAutomationBroker InitializeHueListner(ConsoleColor consoleColor, List<IBroker> brokers = null)
        {
            var notifiers = new List<INotifier>
            {
                new HueNotifier("192.168.1.32", new List<int> {11}),
                new MyStromNotifier("192.168.1.27")
            };

            var listener = new HueListenerService("192.168.1.32", 11);

            brokers = brokers ?? new List<IBroker>();
            var hueAutomation = new HueAutomationBroker(listener, notifiers, brokers);
            hueAutomation.OnMessage += message => OutputMessage(message, consoleColor);

            return hueAutomation;
        }

        private static void OutputMessage(string message, ConsoleColor consoleColor)
        {
            Console.ForegroundColor = consoleColor;
            Console.WriteLine(string.Format("{0}: {1}",DateTime.Now, message));
        }
    }
}
