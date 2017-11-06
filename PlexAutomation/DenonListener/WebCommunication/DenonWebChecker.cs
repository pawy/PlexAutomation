using System;
using System.IO;
using System.Net;
using System.Xml.Serialization;
using DenonListener.XMLContainers;

namespace DenonListener.WebCommunication
{
    public class DenonWebChecker
    {
        private readonly Uri _serverUri;
        public DenonWebChecker(Uri serverUri)
        {
            _serverUri = serverUri;
        }

        public Item Check()
        {
            var request = WebRequest.Create(_serverUri);
            var response = request.GetResponse();

            if (((HttpWebResponse)response).StatusCode == HttpStatusCode.OK)
            {
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);

                XmlSerializer serializer = new XmlSerializer(typeof(Item));
                Item item = (Item)serializer.Deserialize(reader);

                return item;
            }

            throw new Exception(string.Format("ResponseCode: {0}", ((HttpWebResponse)response).StatusCode));
        }
    }
}
