using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuilderUtils.Models
{
    public class ChatbaseRequest
    {
        public CBBoxContent Content { get; set; }

        public ChatbaseRequest()
        {
            Content = new CBBoxContent();
        }
    }


    public partial class CBBoxContent
    {
        [JsonProperty("api_key")]
        public string ApiKey { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("platform")]
        public string Platform { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("not_handled", NullValueHandling = NullValueHandling.Ignore)]
        public bool NotHandled { get; set; }

        [JsonProperty("intent", NullValueHandling = NullValueHandling.Ignore)]
        public string Intent { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("time_stamp")]
        public int TimeStamp { get; set; }
    }
}
