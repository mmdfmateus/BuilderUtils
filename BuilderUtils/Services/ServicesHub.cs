﻿using BuilderUtils.Extensions;
using BuilderUtils.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Lime.Messaging.Contents;
using System.Reflection;
using System.Text;

namespace BuilderUtils.Services
{
    public class ServicesHub : IServicesHub
    {
        private static IBlipBuilderFlowFactory _flowFactory { get; set; }
        private static IChatbaseRequestFactory _chatbaseRequestFactory { get; set; }

        private IChatbaseExtension _chatbaseExtension;
        private ILimeAdapterExtension _limeAdapterExtension;

        public ServicesHub()
        {
            _flowFactory = new BlipBuilderFlowFactory();
            _chatbaseRequestFactory = new ChatbaseRequestFactory();
            _chatbaseExtension = new ChatbaseExtension();
            _limeAdapterExtension = new LimeAdapterExtension();
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
                    catch (Exception)
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

        public static JObject GetChatbaseDeserialized()
        {
            var path = System.AppDomain.CurrentDomain.BaseDirectory + "../../../chatbaserequestbody.json";
            return JsonConvert.DeserializeObject<JObject>(File.ReadAllText(path));
        }

        public void InsertExtrasEventTrack()
        {
            Console.Title = "[BLiP Builder Utils] Creating Extras";
            Console.WriteLine("Write the extras using the following format (comma separated):");
            Console.WriteLine("ExtraKey, ExtraValue");
            Console.WriteLine("The basic extras (userId and originatorMessageId) are added by default");
            Console.WriteLine("When you're done, type '0' (without quotes)");
            var extraInput = Console.ReadLine();
            var extras = new Dictionary<string, string>
            {
                { "userId", "{{contact.identity}}" },
                { "originatorMessageId", "{{input.message@id}}" }
            };

            while (extraInput != "0")
            {
                var tokens = extraInput.Split(',', StringSplitOptions.RemoveEmptyEntries);
                if (tokens.Length != 2)
                {
                    Console.WriteLine($"{extraInput} is not a valid extra pair");
                }
                extras.Add(tokens[0].Trim(), tokens[1].Trim());
                extraInput = Console.ReadLine();
            }

            Console.WriteLine("What is the FULL PATH of the exported .json file?");
            var path = Console.ReadLine();

            var builderFlowJson = GetBuilderFlow(path);
            var flow = _flowFactory.Build(builderFlowJson);
            foreach (var box in flow.Boxes)
            {
                foreach (var item in box.Content)
                {
                    foreach (var leavingCustomActions in item.Value.LeavingCustomActions)
                    {
                        if (leavingCustomActions.Type.Contains("TrackEvent"))
                        {
                            var value = string.Empty;
                            foreach (var extra in extras)
                            {
                                if (!leavingCustomActions.Settings.Extras.ContainsKey(extra.Key))
                                {
                                    leavingCustomActions.Settings.Extras.Add(extra);
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
                            foreach (var extra in extras)
                            {
                                if (!enteringCustomAction.Settings.Extras.ContainsKey(extra.Key))
                                {
                                    enteringCustomAction.Settings.Extras.Add(extra);
                                }
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

        public void InsertChatbaseRequests()
        {
            Console.WriteLine("What is the FULL PATH of the exported .json file?");
            var path = Console.ReadLine();


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
                try
                {
                    var builderFlowJson = GetBuilderFlow(path);
                    var flow = _flowFactory.Build(builderFlowJson);
                    var cbRequestModel = new CBBoxContent();

                    Console.WriteLine("These are the properties from Chatbase requests body:\n");
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("\t\tapi_key:    {{config.chatbaseApiKey}}");
                    Console.WriteLine("\t\tuser_id:    {{contact.identity}}");
                    Console.WriteLine("\t\ttime_stamp: {{calendar.unixTimeMilliseconds}}");
                    Console.WriteLine("\t\tplatform:   {{config.chatbasePlatform}}");
                    Console.WriteLine("\t\tversion:    {{config.chatbaseVersion}}");
                    Console.ResetColor();


                    Console.Write("\nDo you want to configure it? (Y/N): ");
                    var choice = Console.ReadLine();
                    if (choice.ToUpper().Equals("Y"))
                        cbRequestModel = _chatbaseExtension.EditChatbaseProperties();

                    var agentMessages = new ChatbaseRequest();
                    var agentTag = new Tag()
                    {
                        Background = "#ffc700",
                        CanChangeBackground = false,
                        Id = "blip-tag-" + Guid.NewGuid().ToString(),
                        Label = "Chatbase (Bot)",
                    };

                    var userTag = new Tag()
                    {
                        Background = "#a88300",
                        CanChangeBackground = false,
                        Id = "blip-tag-" + Guid.NewGuid().ToString(),
                        Label = "Chatbase (Usuario)",
                    };

                    foreach (var box in flow.Boxes)
                    {
                        foreach (var item in box.Content)
                        {
                            agentMessages.Messages.Clear();
                            foreach (var customAction in item.Value.ContentActions)
                            {

                                if (customAction.Action != null)
                                {

                                    if (customAction.Action.Settings.Type != ChatState.MIME_TYPE)
                                    {
                                        var agentMessage = "";
                                        if (customAction.Action.Type != "SendMessage")
                                        {
                                            agentMessage = customAction.Action.Settings.RawContent;
                                        }
                                        else
                                        {
                                            switch (customAction.Action.Settings.Type)
                                            {
                                                case Select.MIME_TYPE:
                                                    agentMessage = _limeAdapterExtension.QuickReplyToString(customAction.Action.Settings.Content);
                                                    break;
                                                case MediaLink.MIME_TYPE:
                                                    agentMessage = _limeAdapterExtension.MediaLinkToString(customAction.Action.Settings.Content);
                                                    break;
                                                case Lime.Messaging.Contents.Input.MIME_TYPE:
                                                    agentMessage = _limeAdapterExtension.LocationInputToString(customAction.Action.Settings.Content);
                                                    break;
                                                case WebLink.MIME_TYPE:
                                                    agentMessage = _limeAdapterExtension.WebLinkToString(customAction.Action.Settings.Content);
                                                    break;
                                                case "application/vnd.lime.collection+json":
                                                    agentMessage = _limeAdapterExtension.DocumentCollectionToString(customAction.Action.Settings.Content);
                                                    break;
                                                default:
                                                    agentMessage = customAction.Action.CardContent.Document.Content.ToString();
                                                    break;
                                            }
                                        }
                                        agentMessages = _chatbaseExtension.GetAgentBodyRequest(cbRequestModel, agentMessages, message: agentMessage);
                                    }
                                }
                                else if (customAction.Input != null)
                                {

                                    if (customAction.Input.Bypass == false)
                                    {
                                        var cbRequestBody = _chatbaseExtension.GetChatbaseBodyRequest(cbRequestModel, type: "user", message: "{{input.content}}");
                                        item.Value.LeavingCustomActions.Add(_chatbaseExtension.GetUserChatbaseCustomAction(cbRequestBody));
                                        if (item.Value.Tags == null)
                                            item.Value.Tags = new List<Tag>();
                                        item.Value.Tags.Add(userTag);
                                    }
                                }

                            }
                            if (agentMessages.Messages.Count > 0)
                            {
                                item.Value.EnteringCustomActions.Add(_chatbaseExtension.GetAgentChatbaseCustomAction(agentMessages));
                                if (item.Value.Tags == null)
                                    item.Value.Tags = new List<Tag>();
                                if (!item.Value.Tags.Contains(agentTag))
                                    item.Value.Tags.Add(agentTag);
                            }



                        }
                    }

                    flow.ParseProxyIntoFlow();

                    var serialized = string.Empty;
                    foreach (var box in flow.Boxes)
                    {
                        var piece = JsonConvert.SerializeObject(box.Content);
                        serialized = serialized + piece.Substring(1, piece.Length - 2) + ",";
                    }

                    serialized = "{" + serialized.Substring(0, serialized.Length - 1) + "}";
                    var exitName = path.Replace(".json", "_chatbase.json");
                    File.WriteAllText(exitName, serialized);
                    Console.WriteLine($"File saved with Path {exitName}");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }



    }
}
