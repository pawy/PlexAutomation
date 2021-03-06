﻿using System;
using System.Collections.Generic;
using HueListener;
using Notificators;
using Notificators.Hue;
using Notificators.MyStrom;
using PlexListener.PMC;
using DenonListener;
using IPListener;
using PlexListener;

namespace PlexAutomation
{
    class Program
    {
        private const ConsoleColor PlexColor = ConsoleColor.DarkGreen;
        private const ConsoleColor HueColor = ConsoleColor.Yellow;
        private const ConsoleColor TvColor = ConsoleColor.Cyan;
        private const ConsoleColor XboxColor = ConsoleColor.Red;
        private const ConsoleColor DenonColor = ConsoleColor.Magenta;

        private const string PlexIp = "192.168.1.40";
        private const string HueIp = "192.168.1.32";
        private const string TvIp = "192.168.1.25";
        private const string XboxIp = "192.168.1.37";
        private const string MyStromIp = "192.168.1.22";
        private const string DenonIp = "192.168.1.34";

        private const int CinemaHueLamp = 24;
        private const int KellerHueLamp = 6;

        static void Main(string[] args)
        {
            DisableConsoleQuickEdit.Go();

            Console.WriteLine("Plex-Automation-Client starting...");

            PlexAutomationBroker plexAutomation = InitializePlexListener(PlexColor);
            plexAutomation.Start();

            //IPAutomationBroker tvIpAutomation = InitializeTVIpListener(TvColor);
            //tvIpAutomation.Start();

            //IPAutomationBroker xboxIpAutomation = InitializeXboxIpListener(XboxColor);
            //xboxIpAutomation.Start();

            HueAutomationBroker hueAutomation = InitializeHueListner(HueColor);
            hueAutomation.Start();

            DenonAutomationBroker denonAutomation = InitializeDenonListener(DenonColor);
            denonAutomation.Start();

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
                new HueNotifier(HueIp, new List<int> {CinemaHueLamp}),
                new MyStromNotifier(MyStromIp)
            };

            var listener = new PlexListenerService(PlexIp);

            var plexAutomation = new PlexAutomationBroker(listener, notifiers);
            plexAutomation.OnMessage += message => OutputMessage(message, consoleColor);

            return plexAutomation;
        }

        /// <summary>
        /// If denon input is game or cable turn off lights
        /// If plex input is anything else or device is turned of turn on lights
        /// </summary>
        private static DenonAutomationBroker InitializeDenonListener(ConsoleColor consoleColor)
        {
            var notifiers = new List<INotifier>
            {
                new HueNotifier(HueIp, new List<int> {CinemaHueLamp}),
                new MyStromNotifier(MyStromIp)
            };

            var listener = new DenonListenerService(DenonIp);

            var denonAutomation = new DenonAutomationBroker(listener, notifiers);
            denonAutomation.OnMessage += message => OutputMessage(message, consoleColor);

            return denonAutomation;
        }

        /// <summary>
        /// If TV is turned on, turn off lights
        /// If TV is turned off, turn on lights
        /// </summary>
        private static IPAutomationBroker InitializeTVIpListener(ConsoleColor consoleColor)
        {
            var notifiers = new List<INotifier>
            {
                new HueNotifier(HueIp, new List<int> {CinemaHueLamp}),
                new MyStromNotifier(MyStromIp)
            };

            var listener = new IPListenerService(TvIp);

            var ipAutomation = new IPAutomationBroker(listener, notifiers);
            ipAutomation.OnMessage += message => OutputMessage(message, consoleColor);

            OutputMessage(string.Format("Tv {0}", TvIp), consoleColor);

            return ipAutomation;
        }

        /// <summary>
        /// If Xbox is turned on, turn off lights
        /// If Xbox is turned off, turn on lights
        /// </summary>
        private static IPAutomationBroker InitializeXboxIpListener(ConsoleColor consoleColor)
        {
            var notifiers = new List<INotifier>
            {
                new HueNotifier(HueIp, new List<int> {CinemaHueLamp}),
                new MyStromNotifier(MyStromIp)
            };

            var listener = new IPListenerService(XboxIp);

            var ipAutomation = new IPAutomationBroker(listener, notifiers);
            ipAutomation.OnMessage += message => OutputMessage(message, consoleColor);

            OutputMessage(string.Format("Xbox {0}", XboxIp), consoleColor);

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
                new HueNotifier(HueIp, new List<int> {CinemaHueLamp}),
                new MyStromNotifier(MyStromIp)
            };

            var listener = new HueListenerService(HueIp, KellerHueLamp);
            
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
