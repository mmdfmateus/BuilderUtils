using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Converters;
using System.Globalization;
using BuilderUtils;

namespace DrawBlipBuilderFlow
{
    public class Program
    {
        static void Main(string[] args)
        {
            var builderFlowJson = GetBuilderFlow();

            foreach (var box in builderFlowJson)
            {
                if (box.Key.Equals("59413693-0d78-4fdd-aebe-c2c28759825b") || box.Key.Equals("fallback")) continue;

                //ToObject<JProperty>().Name.Equals("input")
                if (bool.Parse(box.Value["$contentActions"].LastOrDefault().FirstOrDefault().FirstOrDefault()["bypass"].ToString()))
                {
                    continue;
                }

                var outputs = box.Value["$conditionOutputs"];
                if (outputs.Count() == 0) continue;

                var extra = builderFlowJson["59413693-0d78-4fdd-aebe-c2c28759825b"]["$conditionOutputs"].FirstOrDefault();
                extra["$connId"] = null;
                extra["$$hashKey"] = null;


                extra["stateId"] = box.Key;

                var obj = JsonConvert.DeserializeObject<Conditions>(extra.ToString());
                obj.ConditionsConditions[0].Values.RemoveAll((p) => true);
                obj.ConditionsConditions[0].Values.Add(box.Value["$title"].ToString());
                obj.ConditionsConditions[0].HashKey = null;
                extra["conditions"] = (JArray.Parse(obj.ConditionsConditions.ToArray().ToJson()));

                builderFlowJson["59413693-0d78-4fdd-aebe-c2c28759825b"]["$conditionOutputs"].FirstOrDefault().AddAfterSelf(extra);
            }
            var serialized = JsonConvert.SerializeObject(builderFlowJson);
            File.WriteAllText("builderflow1.json", serialized);
        }

        public static JObject GetBuilderFlow()
        {
            return JsonConvert.DeserializeObject<JObject>(File.ReadAllText("builderflow.json"));
        }
    }
}
