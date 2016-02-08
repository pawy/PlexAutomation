﻿using System;
using System.IO;
using System.Net;
using System.Xml.Serialization;
using PlexListener.XMLContainers;

namespace PlexListener.WebCommunication
{
    public class PlexWebChecker
    {
        private readonly Uri _serverUri;
        public PlexWebChecker(Uri serverUri)
        {
            _serverUri = serverUri;
        }

        public MediaContainer Check()
        {
            var request = WebRequest.Create(_serverUri);
            var response = request.GetResponse();

            if (((HttpWebResponse)response).StatusCode == HttpStatusCode.OK)
            {
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);

                XmlSerializer serializer = new XmlSerializer(typeof(MediaContainer));
                MediaContainer mediaContainer = (MediaContainer)serializer.Deserialize(reader);

                return mediaContainer;
            }

            throw new Exception(((HttpWebResponse)response).StatusCode.ToString());
        }
    }
}
