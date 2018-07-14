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

            foreach (BoxProxy proxy in flow.Proxy)
            {
                if (proxy.Key.Equals("59413693-0d78-4fdd-aebe-c2c28759825b") || proxy.Key.Equals("fallback")) continue;
                if (proxy.Content.ContentActions.FirstOrDefault(a => a.Input != null).Input.Bypass) continue;

                var outputs = proxy.Content.ConditionOutputs;
                if (outputs.Count() == 0) continue;
                var extraConditionOutput = new ConditionOutput
                {
                    ConnId = null,
                    StateId = proxy.Key,
                    Conditions = new List<Condition>()
                };
                var extraCondition = new Condition
                {
                    Comparison = "equals",
                    Source = "context",
                    Values = new List<string>(),
                    Variable = "redirect"
                };
                extraCondition.Values.Add(proxy.Content.Title);

                extraConditionOutput.Conditions.Add(extraCondition);

                flow.Proxy.FirstOrDefault(b => b.Key.Equals("59413693-0d78-4fdd-aebe-c2c28759825b")).Content.ConditionOutputs.Add(extraConditionOutput);
            }
            flow.ParseProxyIntoFlow();

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
