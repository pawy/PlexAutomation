using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HueListener.MessageObjects
{
    public class Light
    {
        [JsonProperty("state")]
        public State State { get; set; }
    }
}
