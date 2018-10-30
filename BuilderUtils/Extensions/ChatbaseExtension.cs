using BuilderUtils.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BuilderUtils.Extensions
{
    public class ChatbaseExtension
    {

        public ChatbaseRequest GetChatbaseBodyRequest(CBBoxContent bodyContentModel, ChatbaseRequest cbRequest = null, string type = "", string message = "", bool notHandled = false, string intent = "")
        {

            if (cbRequest == null)
                cbRequest = new ChatbaseRequest();

            var bodyContent = new CBBoxContent()
            {
                ApiKey = bodyContentModel.ApiKey,
                Platform = bodyContentModel.Platform,
                TimeStamp = bodyContentModel.TimeStamp,
                UserId = bodyContentModel.UserId,
                Version = bodyContentModel.Version,
            };

            bodyContent.Type = (type == "") ? "agent" : type;
            bodyContent.Message = (message == "") ? "{{input.content}}" : message;
            bodyContent.Intent = (intent == "") ? "{{input.intent.name}}" : intent;
            bodyContent.NotHandled = (notHandled) ? notHandled : false;

            cbRequest.Messages.Add(bodyContent);

            return cbRequest;
        }

        public ChatbaseRequest GetAgentBodyRequest(CBBoxContent bodyContentModel, ChatbaseRequest cbRequest = null, string message = "", bool notHandled = false, string intent = "")
        {

            if (cbRequest == null || cbRequest.Messages.Count == 0)
                cbRequest = new ChatbaseRequest();

            var bodyContent = new CBBoxContent()
            {
                ApiKey = bodyContentModel.ApiKey,
                Platform = bodyContentModel.Platform,
                TimeStamp = bodyContentModel.TimeStamp,
                UserId = bodyContentModel.UserId,
                Version = bodyContentModel.Version,
            };

            bodyContent.Type = "agent";
            bodyContent.Message = (message == "") ? "{{input.content}}" : message;
            bodyContent.Intent = (intent == "") ? "{{input.intent.name}}" : intent;

            cbRequest.Messages.Add(bodyContent);

            return cbRequest;
        }

        public CustomAction GetAgentChatbaseCustomAction(ChatbaseRequest cbRequest, string agentMessage = "", string type = null, string message = null)
        {

            var action = new CustomAction()
            {
                Type = "ProcessHttp",
                Title = "Chatbase Event Tracking - Agent",
                Invalid = false,
                Settings = new CustomActionSettings()
                {
                    Headers = { },
                    Method = "POST",
                    Body = JsonConvert.SerializeObject(cbRequest).Replace("\"{{calendar.unixTimeMilliseconds}}\"", "{{calendar.unixTimeMilliseconds}}"),
                    Uri = "{{config.chatbaseUrl}}",
                    ResponseStatusVariable = "chatbaseResponseStatus",
                    ResponseBodyVariable = "chatbaseResponseBody"
                }
            };


            return action;
        }

        public CustomAction GetUserChatbaseCustomAction(ChatbaseRequest cbRequest)
        {
            var action = new CustomAction()
            {
                Type = "ProcessHttp",
                Title = "Chatbase Event Tracking - User",
                Invalid = false,
                Settings = new CustomActionSettings()
                {
                    Headers = { },
                    Method = "POST",
                    Body = JsonConvert.SerializeObject(cbRequest).Replace("\"{{calendar.unixTimeMilliseconds}}\"", "{{calendar.unixTimeMilliseconds}}"),
                    Uri = "{{config.chatbaseUrl}}",
                    ResponseStatusVariable = "chatbaseResponseStatus",
                    ResponseBodyVariable = "chatbaseResponseBody"
                }
            };

            return action;
        }

        private static void ClearCurrentLine()
        {
            Console.SetCursorPosition(0, Console.CursorTop - 1);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, Console.CursorTop - 1);
        }

        public CBBoxContent EditChatbaseProperties()
        {
            var cbBoxContent = new CBBoxContent()
            {
                ApiKey = "{{config.chatbaseApiKey}}",
                UserId = "{{contact.identity}}",
                TimeStamp = "{{calendar.unixTimeMilliseconds}}",
                Platform = "{{config.chatbasePlatform}}",
                Version = "{{config.chatbaseVersion}}"
            };

            Console.WriteLine("\nSelect which of the properties below you want to edit with (Y/N)\n");

            #region Iteration through each propertie

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write($"\t\tapi_key:    {cbBoxContent.ApiKey}  ");
            Console.ResetColor();
            if (Console.ReadLine().ToString().ToUpper().Equals("Y"))
            {
                ClearCurrentLine();
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write("\t\tapi_key:    ");
                cbBoxContent.ApiKey = Console.ReadLine();
            }
            else
            {
                ClearCurrentLine();
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine($"\t\tapi_key:    {cbBoxContent.ApiKey}  ");
            }

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write($"\t\tuser_id:    {cbBoxContent.UserId}  ");
            Console.ResetColor();
            if (Console.ReadLine().ToUpper().Equals("Y"))
            {
                ClearCurrentLine();
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("\t\tuser_id:    ");
                cbBoxContent.UserId = Console.ReadLine();
            }
            else
            {
                ClearCurrentLine();
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine($"\t\tuser_id:    {cbBoxContent.UserId}  ");
            }

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write($"\t\ttime_stamp: {cbBoxContent.TimeStamp}  ");
            Console.ResetColor();
            if (Console.ReadLine().ToUpper().Equals("Y"))
            {
                ClearCurrentLine();
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write("\t\ttime_stamp: ");
                cbBoxContent.TimeStamp = Console.ReadLine();
            }
            else
            {
                ClearCurrentLine();
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine($"\t\ttime_stamp: {cbBoxContent.TimeStamp}  ");
            }

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write($"\t\tplatform:   {cbBoxContent.Platform}  ");
            Console.ResetColor();
            if (Console.ReadLine().ToUpper().Equals("Y"))
            {
                ClearCurrentLine();
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write("\t\tplatform:   ");
                cbBoxContent.Platform = Console.ReadLine();
            }
            else
            {
                ClearCurrentLine();
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine($"\t\tplatform:   {cbBoxContent.Platform}  ");
            }

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write($"\t\tversion:    {cbBoxContent.Version}  ");
            Console.ResetColor();
            if (Console.ReadLine().ToUpper().Equals("Y"))
            {
                ClearCurrentLine();
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write("\t\tversion:    ");
                cbBoxContent.Version = Console.ReadLine();
            }
            else
            {
                ClearCurrentLine();
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine($"\t\tversion:    {cbBoxContent.Version}  ");
            }


            Console.WriteLine();
            Console.ResetColor();

            #endregion

            return cbBoxContent;
        }
    }
}
