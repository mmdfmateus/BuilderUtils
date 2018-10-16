using BuilderUtils.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BuilderUtils.Extensions
{
    public class ChatbaseExtension
    {

        public ChatbaseRequest GetChatbaseBodyRequest(ChatbaseRequest cbRequest = null, string type = "", string message = "", bool notHandled = false, string intent = "")
        {

            if (cbRequest == null)
                cbRequest = new ChatbaseRequest();
            cbRequest.Messages.Add(new CBBoxContent()
            {
                ApiKey = "{{config.chatbaseApiKey}}",
                Type = (type == "") ? "agent" : type,
                Platform = "{{config.chatbasePlatform}}",
                Message = (message == "") ? "{{input.content}}" : message,
                Intent = (intent == "") ? "" : intent,
                NotHandled = (notHandled) ? notHandled : false,
                Version = "{{config.chatbaseVersion}}",
                UserId = "{{contact.identity}}",
                TimeStamp = "{{calendar.unixTimeMilliseconds}}",
            });

            return cbRequest;
        }

        public ChatbaseRequest GetAgentBodyRequest(ChatbaseRequest cbRequest = null, string message = "", bool notHandled = false, string intent = "")
        {

            if (cbRequest == null || cbRequest.Messages.Count == 0)
                cbRequest = new ChatbaseRequest();
            cbRequest.Messages.Add(new CBBoxContent()
            {
                ApiKey = "{{config.chatbaseApiKey}}",
                Type = "agent",
                Platform = "{{config.chatbasePlatform}}",
                Message = (message == "") ? "" : message,
                //Intent = (intent == "") ? "" : intent,
                //NotHandled = (notHandled) ? notHandled : false,
                Version = "{{config.chatbaseVersion}}",
                UserId = "{{contact.identity}}",
                TimeStamp = "{{calendar.unixTimeMilliseconds}}",
            });

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
                    Body = JsonConvert.SerializeObject(cbRequest),
                    Uri = "{{config.chatbaseUrl}}"
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
                    Body = JsonConvert.SerializeObject(cbRequest),
                    Uri = "{{config.chatbaseUrl}}"
                }
            };

            return action;
        }

    }
}
