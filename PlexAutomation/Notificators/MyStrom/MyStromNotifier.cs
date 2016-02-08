using System;
using System.Net.Http;

namespace Notificators.MyStrom
{
    public class MyStromNotifier : INotifier
    {
        public Uri DeviceUri { get; private set; }

        public MyStromNotifier(Uri deviceUri)
        {
            DeviceUri = deviceUri;
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
            GetRequest("relay?state=0");
        }

        private void TurnOn()
        {
            GetRequest("relay?state=1");
        }

        private async void GetRequest(string getParams)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = DeviceUri;
                await client.GetAsync(getParams);
            }
        }
    }
}
