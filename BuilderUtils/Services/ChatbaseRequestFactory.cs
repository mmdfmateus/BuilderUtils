using System;
using System.Collections.Generic;
using System.Text;
using BuilderUtils.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BuilderUtils.Services
{
    public class ChatbaseRequestFactory : IChatbaseRequestFactory
    {
        public ChatbaseRequest Build(JObject deserializedJsonFlow)
        {
            var flow = new ChatbaseRequest();
            flow.Content = JsonConvert.DeserializeObject<CBBoxContent>(JsonConvert.SerializeObject(deserializedJsonFlow));

            return flow;
        }
    }
}
