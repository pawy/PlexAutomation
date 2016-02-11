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
        private static PlexAutomationBroker _plexAutomationBroker;
        private static HueAutomationBroker _huerHueAutomationBroker;

        private const ConsoleColor PlexColor = ConsoleColor.DarkGreen;
        private const ConsoleColor HueColor = ConsoleColor.Yellow;

        static void Main(string[] args)
        {
            Console.WriteLine("Plex-Automation-Client starting...");

            PlexAutomationBroker plexAutomation = InitializePlexListener(PlexColor);
            HueAutomationBroker hueAutomation = InitializeHueListner(HueColor, plexAutomation);

            hueAutomation.Start();

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Press <ENTER> to stop the Plex-Automation-Client");
            Console.ReadKey();
        }

        /// <summary>
        /// If plex starts playing turn off lights, if it stops turn on lights
        /// </summary>
        private static PlexAutomationBroker InitializePlexListener(ConsoleColor consoleColor)
        {
            //Create Notifiers
            var notifiers = new List<INotifier>
            {
                new HueNotifier("192.168.1.32", new List<int> {11, 12}),
                new MyStromNotifier("192.168.1.27")
            };

            //Create Listener
            var listener = new PlexListenerService("192.168.1.31");

            var plexAutomation = new PlexAutomationBroker(listener, notifiers);
            plexAutomation.OnMessage += message => OutputMessage(message, consoleColor);

            return plexAutomation;
        }

        /// <summary>
        /// If hue is turned on, then turn two lamps blue and enable MyStrom
        /// Also turn on the PlexListener
        /// </summary>
        private static HueAutomationBroker InitializeHueListner(ConsoleColor consoleColor, PlexAutomationBroker plexAutomation)
        {
            //Create Notifiers
            var notifiers = new List<INotifier>
            {
                new HueNotifier("192.168.1.32", new List<int> {11, 12}),
                new MyStromNotifier("192.168.1.27")
            };

            //Create Listener
            var listener = new HueListenerService("192.168.1.32", 11);

            var hueAutomation = new HueAutomationBroker(listener, notifiers, new List<IBroker>{ plexAutomation });
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
