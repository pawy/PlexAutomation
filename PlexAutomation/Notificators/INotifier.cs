using System;

namespace Notificators
{
    public interface INotifier
    {
        void Notify(EventType eventType);
    }
}
