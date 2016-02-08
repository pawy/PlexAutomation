using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlexListener
{
    public interface IPLexListener
    {
        void StartListener();

        event EventHandler<PlexNotificationEventArgs> OnNewNotification;
    }
}
