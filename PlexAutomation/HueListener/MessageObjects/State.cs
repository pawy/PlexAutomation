using Newtonsoft.Json;

namespace HueListener.MessageObjects
{
    public class State
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

        [JsonProperty("reachable")]
        public bool Reachable { get; set; }
    }
}
