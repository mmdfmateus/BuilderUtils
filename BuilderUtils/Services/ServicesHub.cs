using BuilderUtils.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BuilderUtils.Services
{
    public class ServicesHub : IServicesHub
    {
        private static IBlipBuilderFlowFactory _flowFactory { get; set; }

        public ServicesHub()
        {
            _flowFactory = new BlipBuilderFlowFactory();
        }
        public void CreateOutputHub()
        {
            Console.Title = "[BLiP Builder Utils] Creating Output Hub";
            Console.WriteLine("What is the ID (state.id) of the flow you want to use as a HUB?");
            var stateId = Console.ReadLine();
            Console.WriteLine("What is the variable name to be used as the EQUALS conditional?");
            var conditionalVariable = Console.ReadLine();
            Console.WriteLine("What is the FULL PATH of the exported .json file?");
            var path = Console.ReadLine();
            AddUniversalHubUsingVariable(stateId, conditionalVariable, path);
        }

        private static void AddUniversalHubUsingVariable(string stateId, string conditionalVariable, string path)
        {
            while (!Path.GetExtension(path).Equals(".json"))
            {
                Console.Beep();
                Console.Write("File is not a JSON. Retry? (Y/N): ");
                var answer = Console.ReadLine();
                if (answer.ToUpper().Equals("Y")) path = Console.ReadLine();
                else break;
            }
            if (Path.GetExtension(path).Equals(".json"))
            {
                var builderFlowJson = GetBuilderFlow(path);
                var flow = new BlipBuilderFlow();

                try
                {
                    flow = _flowFactory.Build(builderFlowJson);
                }
                catch (Exception ex)
                {
                    Console.Beep();
                    Console.WriteLine("File is not a valid Builder Flow");
                }
                var filename = Path.GetFileNameWithoutExtension(path);

                foreach (BoxProxy proxy in flow.Proxy)
                {
                    if (proxy.Key.Equals(stateId) || proxy.Key.Equals("fallback")) continue;
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
                        Variable = conditionalVariable
                    };
                    extraCondition.Values.Add(proxy.Content.Title);

                    extraConditionOutput.Conditions.Add(extraCondition);

                    flow.Proxy.FirstOrDefault(b => b.Key.Equals(stateId)).Content.ConditionOutputs.Add(extraConditionOutput);
                }
                flow.ParseProxyIntoFlow();

                var serialized = string.Empty;
                foreach (var box in flow.Boxes)
                {
                    var piece = JsonConvert.SerializeObject(box.Content);
                    serialized = serialized + piece.Substring(1, piece.Length - 2) + ",";
                }

                serialized = "{" + serialized.Substring(0, serialized.Length - 1) + "}";
                var exitName = Path.GetFullPath(path).Replace(Path.GetFileName(path), "") + filename + "EDIT.json";
                File.WriteAllText(exitName, serialized);
                Console.WriteLine($"File saved with Path {exitName}");
            }
        }

        public static JObject GetBuilderFlow(string path)
        {
            return JsonConvert.DeserializeObject<JObject>(File.ReadAllText(path));
        }
    }
}
