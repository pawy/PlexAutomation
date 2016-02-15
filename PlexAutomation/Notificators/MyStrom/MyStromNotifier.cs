using System;
using System.Net.Http;

namespace Notificators.MyStrom
{
    public class MyStromNotifier : INotifier
    {
        public string SwitchIp { get; private set; }

        public MyStromNotifier(string switchIp)
        {
            SwitchIp = switchIp;
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
            return string.Format("MyStrom Switch {0}", SwitchIp);
        }

        private void TurnOff()
        {
            GetRequest("relay?state=0");
        }

        private void TurnOn()
        {
            GetRequest("relay?state=1");
        }

        private void GetRequest(string getParams)
        {
            bool done = false;
            int retries = 0;
            while (done == false)
            {
                try
                {
                    using (var client = new HttpClient())
                    {
                        client.Timeout = new TimeSpan(0, 0, 5);
                        client.BaseAddress = new Uri(string.Format("http://{0}", SwitchIp));
                        client.GetAsync(getParams).Wait();
                        done = true;
                    }
                }
                catch (Exception)
                {
                    if (retries == 3)
                        throw;

                    retries++;
                }
            }
        }
    }
}
