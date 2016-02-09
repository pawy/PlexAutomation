namespace Notificators
{
    public interface INotifier
    {
        void Notify(EventType eventType);
        string GetDisplayName();

    }
}
