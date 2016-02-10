using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Notificators.Hue.MessageObjects;

namespace Notificators.Hue
{
    public class HueNotifier : INotifier
    {
        public string BridgeIp { get; private set; }
        public List<int> Lamps { get; private set; }

        public State StateConfiguration { get; private set; }

        public HueNotifier(string bridgeIp, List<int> lamps, State lampStateConfiguration = null)
        {
            BridgeIp = bridgeIp;
            Lamps = lamps;
            StateConfiguration = lampStateConfiguration ?? State.LightBlue;
        }

        public void Notify(EventType eventType)
        {
            switch (eventType)
            {
                case EventType.Playing:
                    TurnOff();
                    break;
                case EventType.NotPlaying:
                    TurnOn();
                    break;
            }
        }

        public string GetDisplayName()
        {
            return string.Format("Hue Lamps {0}", string.Join(", ", Lamps));
        }

        private void TurnOff()
        {
            PostRequest(State.Off);
        }

        private void TurnOn()
        {
            PostRequest(StateConfiguration);
        }

        private void PostRequest(State state)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(string.Format("http://{0}/api/newdeveloper/lights/",BridgeIp));

                foreach (int lamp in Lamps)
                {
                    StringContent content = new StringContent(state.ToJson(), Encoding.UTF8, "application/json");
                    client.PutAsync(string.Format("{0}/state",lamp),content).Wait();
                }
            }
        }
    }
}
