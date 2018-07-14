using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BuilderUtils.Models;
using BuilderUtils.Services;

namespace DrawBlipBuilderFlow
{
    public class Program
    {
        static void Main(string[] args)
        {
            var builderFlowJson = GetBuilderFlow();
            var flowFactory = new BlipBuilderFlowFactory();
            var flow = flowFactory.Build(builderFlowJson);

            //foreach (Box flowBox in flow.Boxes)
            //{
            //    if (flowBox..Equals("59413693-0d78-4fdd-aebe-c2c28759825b") || flowBox.BoxKey.Equals("fallback")) continue;
            //    if (flowBox.BoxValue.ContentActions.FirstOrDefault(a => a.Input != null).Input.Bypass) continue;

            //    var outputs = flowBox.BoxValue.ConditionOutputs;
            //    if (outputs.Count() == 0) continue;
            //    var extraConditionOutput = new ConditionOutput
            //    {
            //        ConnId = null,
            //        StateId = flowBox.BoxKey,
            //        Conditions = new List<Condition>()
            //    };
            //    var extraCondition = new Condition
            //    {
            //        Comparison = "equals",
            //        Source = "context",
            //        Values = new List<string>(),
            //        Variable = "redirect"
            //    };
            //    extraCondition.Values.Add(flowBox.BoxValue.Title);

            //    extraConditionOutput.Conditions.Add(extraCondition);

            //    flow.Boxes.FirstOrDefault(b => b.BoxKey.Equals("59413693-0d78-4fdd-aebe-c2c28759825b")).BoxValue.ConditionOutputs.Add(extraConditionOutput);
            //}

            //foreach (var box in builderFlowJson)
            //{
            //    if (box.Key.Equals("59413693-0d78-4fdd-aebe-c2c28759825b") || box.Key.Equals("fallback")) continue;

            //    //ToObject<JProperty>().Name.Equals("input")
            //    if (bool.Parse(box.Value["$contentActions"].LastOrDefault().FirstOrDefault().FirstOrDefault()["bypass"].ToString()))
            //    {
            //        continue;
            //    }

            //    var outputs = box.Value["$conditionOutputs"];
            //    if (outputs.Count() == 0) continue;

            //    var extra = builderFlowJson["59413693-0d78-4fdd-aebe-c2c28759825b"]["$conditionOutputs"].FirstOrDefault();
            //    extra["$connId"] = null;
            //    extra["$$hashKey"] = null;


            //    extra["stateId"] = box.Key;

            //    var obj = JsonConvert.DeserializeObject<Conditions>(extra.ToString());
            //    obj.ConditionsConditions[0].Values.RemoveAll((p) => true);
            //    obj.ConditionsConditions[0].Values.Add(box.Value["$title"].ToString());
            //    obj.ConditionsConditions[0].HashKey = null;
            //    extra["conditions"] = (JArray.Parse(obj.ConditionsConditions.ToArray().ToJson()));

            //    builderFlowJson["59413693-0d78-4fdd-aebe-c2c28759825b"]["$conditionOutputs"].FirstOrDefault().AddAfterSelf(extra);
            //}
            var serialized = string.Empty;
            foreach (var box in flow.Boxes)
            {
                var piece = JsonConvert.SerializeObject(box.Content);
                serialized = serialized + piece.Substring(1, piece.Length-2) + ",";
            }

            serialized = "{"+serialized.Substring(0, serialized.Length-1)+"}";
            File.WriteAllText("builderflow1.json", serialized);
        }

        public static JObject GetBuilderFlow()
        {
            return JsonConvert.DeserializeObject<JObject>(File.ReadAllText("builderflow.json"));
        }
    }
}
