using Newtonsoft.Json;

namespace Notificators.Hue.MessageObjects
{
    public abstract class BaseMessage
    {
        public virtual string ToJson()
        {
            return JsonConvert.SerializeObject(this,
                Formatting.None,
                            new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore
                            });
        }
    }
}
