using System.Net.NetworkInformation;
namespace IPListener.Communication
{
    public class IPChecker
    {
        private readonly string _ip;
        public IPChecker(string ip)
        {
            _ip = ip;
        }

        public bool Check()
        {
            Ping p = new Ping();
            PingReply reply = p.Send(_ip, 3000);
            if (reply.Status == IPStatus.Success || reply.Status == IPStatus.TimedOut)
            {
                return true;
            }
            return false;
        }
    }
}
