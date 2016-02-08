using System;
using System.IO;
using System.Xml.Serialization;
using PlexListener.PMS.XMLContainers;

namespace PlexListener.PMS.WebCommunication
{
    public class PlexFileChecker
    {
        private readonly Uri _serverUri;
        public PlexFileChecker(Uri serverUri)
        {
            _serverUri = serverUri;
        }

        public MediaContainer Check()
        {
            using (FileStream stream = new FileStream(_serverUri.LocalPath, FileMode.Open))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(MediaContainer));
                return (MediaContainer)serializer.Deserialize(stream);
            }
        }
    }
}
