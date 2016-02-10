using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using HueListener.MessageObjects;
using Newtonsoft.Json;

namespace HueListener.WebCommunication
{
    public class HueWebChecker
    {
        private readonly string _bridgeIp;
        private readonly int _lamp;
        public HueWebChecker(string bridgeIp, int lamp)
        {
            _bridgeIp = bridgeIp;
            _lamp = lamp;
        }

        public Light Check()
        {
            using (var client = new HttpClient())
            {
                var json = client.GetStringAsync(string.Format("http://{0}/api/newdeveloper/lights/{1}", _bridgeIp, _lamp)).Result;
                //var json = File.ReadAllText(@"C:\Users\Pascal\Desktop\huelight.txt");
                return JsonConvert.DeserializeObject<Light>(json);
            }
        }
    }
}
