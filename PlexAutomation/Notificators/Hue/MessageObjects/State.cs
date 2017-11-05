using Newtonsoft.Json;

namespace Notificators.Hue.MessageObjects
{
    public class State : BaseMessage
    {
        [JsonProperty("on")]
        public bool On { get; set; }

        [JsonProperty("bri")]
        public int? Brightness { get; set; }

        [JsonProperty("xy")]
        public double[] XyColorCode
        {
            get
            {
                if (X == null)
                {
                    return null;
                }
                return new double[2] {X.GetValueOrDefault(0), Y.GetValueOrDefault(0)};
            }
        }

        [JsonIgnore]
        public double? X { get; set; }
        [JsonIgnore]
        public double? Y { get; set; }

        public static State LightBlue
        {
            get
            {
                return new State
                {
                    Brightness = 255,
                    On = true,
                    X = 0.2038,
                    Y = 0.4767
                };
            }
        }

        public static State Off
        {
            get
            {
                return new State
                {
                    On = false
                };
            }
        }
    }
}
