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

        public void CreateOutputHub(bool verbose, string stateId, string conditionalVariable, string path)
        {
            AddUniversalHubUsingVariable(stateId, conditionalVariable, path, verbose);
        }

        private static void AddUniversalHubUsingVariable(string stateId, string conditionalVariable, string path, bool verbose = false)
        {
            int count = 0;
            while (!Path.GetExtension(path).Equals(".json"))
            {
                Console.Beep();
                if (verbose) Console.Write($"File {path} is not a JSON. Retry? (Y/N): ");
                else Console.Write("File is not a JSON. Retry? (Y/N): ");

                var answer = Console.ReadLine();
                if (answer.ToUpper().Equals("Y")) path = Console.ReadLine();
                else break;
            }
            if (Path.GetExtension(path).Equals(".json"))
            {
                try
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
                        if (verbose) Console.Write($"File {path} is not a valid Builder Flow");
                        else Console.WriteLine("File is not a valid Builder Flow");
                    }
                    var filename = Path.GetFileNameWithoutExtension(path);

                    foreach (BoxProxy proxy in flow.Proxy)
                    {
                        if (proxy.Key.Equals(stateId) || proxy.Key.Equals("fallback")) continue;
                        if (proxy.Content.ContentActions.FirstOrDefault(a => a.Input != null).Input.Bypass) continue;

                        var outputs = proxy.Content.ConditionOutputs;
                        if (outputs.Count() == 0) continue;
                        VerboseServices.LogVerboseLine(verbose, $"> Creating output from {stateId} to {proxy.Content.Title}...");
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
                        VerboseServices.LogVerboseLine(verbose, $">> Successfully created output from {stateId} to {proxy.Content.Title}.");
                        count++;
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
                    VerboseServices.LogVerboseLine(verbose, $">>> Successfully created output hub on {stateId} to {count} boxes.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

        public static JObject GetBuilderFlow(string path)
        {
            return JsonConvert.DeserializeObject<JObject>(File.ReadAllText(path));
        }

        public void InsertExtrasEventTrack(Dictionary<string, string> Extras)
        {
            Console.WriteLine("What is the FULL PATH of the exported .json file?");
            var path = Console.ReadLine();
            var builderFlowJson = GetBuilderFlow(path);
            var flow = _flowFactory.Build(builderFlowJson);
            foreach (var box in flow.Boxes)
            {
                foreach (var item in box.Content)
                {
                    if (!item.Value.Title.Contains("Início") && 
                        !item.Value.Title.Contains("Exceções") && 
                        !item.Value.Title.ToLower().Contains("trk") && 
                        !item.Value.Title.ToLower().Contains("http") &&
                        !item.Value.Title.ToLower().Contains("save data") &&
                        !item.Value.Title.ToLower().Contains("altera canal") &&
                        !item.Value.Title.ToLower().Contains("3.1 valida statuscodenlp") &&
                        !item.Value.Title.ToLower().Contains("fluxo erro chamada api") &&
                        !item.Value.Title.ToLower().Contains("4. 1 intenção encontrada") &&
                        !item.Value.Title.ToLower().Contains("valida entidades") &&
                        !item.Value.Title.ToLower().Contains("4.Valida Intenção") &&
                        !item.Value.Title.ToLower().Contains("4.1.1.a valida intenção com contexto") &&
                        !item.Value.Title.ToLower().Contains("4.a resposta smalltalk"))
                    {
                        if (item.Value.LeavingCustomActions == null)
                            item.Value.LeavingCustomActions = new List<CustomAction>();

                        item.Value.LeavingCustomActions.Add(new CustomAction()
                        {
                            HashKey = null,
                            Invalid = false,
                            Type = "TrackEvent",
                            Title = "Registro de eventos",
                            Settings = new CustomActionSettings()
                            {
                                Category = "Mensagens por canal",
                                Action = "{{contact.extras.Canal}}",
                                Extras = new Dictionary<string, string>()
                            }

                        });
                    }

                    foreach (var leavingCustomActions in item.Value.LeavingCustomActions)
                    {
                        if (leavingCustomActions.Type.Contains("TrackEvent"))
                        {
                            var value = string.Empty;
                            foreach (var extra in Extras)
                            {
                                if (extra.Key.Contains("Regional do usuario"))
                                {
                                    if (leavingCustomActions.Settings.Extras.TryGetValue(extra.Key, out value))
                                        leavingCustomActions.Settings.Extras[extra.Key] = value;
                                    else
                                        leavingCustomActions.Settings.Extras.Add(extra.Key, extra.Value);
                                }
                                else if (extra.Key.Contains("Canal"))
                                {
                                    if (leavingCustomActions.Settings.Extras.TryGetValue(extra.Key, out value))
                                        leavingCustomActions.Settings.Extras[extra.Key] = "{{contact.extras.Canal}}";
                                    else
                                        leavingCustomActions.Settings.Extras.Add(extra.Key, extra.Value);

                                }
                                else
                                {
                                    if (!leavingCustomActions.Settings.Extras.TryGetValue(extra.Key, out value))
                                        leavingCustomActions.Settings.Extras.Add(extra.Key, extra.Value);
                                }
                            }

                            Console.WriteLine(leavingCustomActions.Title);
                        }
                    }

                    foreach (var enteringCustomAction in item.Value.EnteringCustomActions)
                    {
                        if (enteringCustomAction.Type.Contains("TrackEvent"))
                        {
                            var value = string.Empty;
                            foreach (var extra in Extras)
                            {
                                if (!enteringCustomAction.Settings.Extras.TryGetValue(extra.Key, out value))
                                    enteringCustomAction.Settings.Extras.Add(extra.Key, extra.Value);
                            }

                            Console.WriteLine(enteringCustomAction.Title);
                        }
                    }
                }
            }

            var serialized = string.Empty;
            foreach (var box in flow.Boxes)
            {
                var piece = JsonConvert.SerializeObject(box.Content);
                serialized = serialized + piece.Substring(1, piece.Length - 2) + ",";
            }

            serialized = "{" + serialized.Substring(0, serialized.Length - 1) + "}";

            path = $"{Path.GetDirectoryName(path)}\\flow.json";
            Console.WriteLine(path);
            System.IO.File.WriteAllText(path, serialized);
        }
    }
}
