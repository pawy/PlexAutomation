using System;
using System.Collections.Generic;

namespace PlexListener
{
    public class PlexListenerConfig
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="serverUri">The url of the server interface (e.g.: http://127.0.0.1:3400)</param>
        public PlexListenerConfig(Uri serverUri)
        {
            ServerUri = serverUri;
            PlayerIps = new List<string>();
        }
        /// <summary>
        /// The url of the server interface
        /// e.g.: http://127.0.0.1:3400
        /// </summary>
        public Uri ServerUri { get; private set; }

        /// <summary>
        /// Restrict to Ips on which the Listener throws a notification
        /// </summary>
        public List<string> PlayerIps { get; set; }
    }
}
