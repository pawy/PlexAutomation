using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Notificators.Hue
{
    public class HueNotifier : INotifier
    {
        public string BridgeIp { get; private set; }
        public List<int> Lamps { get; private set; } 

        public HueNotifier(string bridgeIp, List<int> lamps)
        {
            BridgeIp = bridgeIp;
            Lamps = lamps;
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

        private void TurnOff()
        {
            PostRequest("{\"on\":false}");
        }

        private void TurnOn()
        {
            PostRequest("{\"on\":true}");
        }

        private async void PostRequest(string data)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(string.Format("http://{0}/api/newdeveloper/lights/",BridgeIp));

                foreach (int lamp in Lamps)
                {
                    StringContent content = new System.Net.Http.StringContent(data, Encoding.UTF8, "application/json");
                    await client.PutAsync(string.Format("{0}/state",lamp),content);
                }
            }
        }
    }
}
