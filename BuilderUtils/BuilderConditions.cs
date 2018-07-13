using DrawBlipBuilderFlow;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace BuilderUtils
{
    public partial class Conditions
    {
        [JsonProperty("conditions")]
        public List<Condition> ConditionsConditions { get; set; }
    }

    public partial class Condition
    {
        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("comparison")]
        public string Comparison { get; set; }

        [JsonProperty("values")]
        public List<string> Values { get; set; }

        [JsonProperty("$$hashKey")]
        public string HashKey { get; set; }

        [JsonProperty("variable")]
        public string Variable { get; set; }
    }

    public partial class Conditions
    {
        public static Conditions FromJson(string json) => JsonConvert.DeserializeObject<Conditions>(json);
    }


    public static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters = {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }


    public static class Serialize
    {
        public static string ToJson(this Condition[] self) => JsonConvert.SerializeObject(self);
    }
}

