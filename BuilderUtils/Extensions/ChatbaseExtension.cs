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
        public ChatbaseRequest GetChatbaseBodyRequest(ChatbaseRequest cbRequest = null, string type = "", string message = "", bool notHandled = false)
        {
            if (cbRequest == null)
                cbRequest = new ChatbaseRequest()
                {
                    Content = new CBBoxContent()
                    {
                        ApiKey = "{{config.chatbaseUrl}}",
                        Type = (type == "") ? "agent" : type,
                        Platform = "WhatsApp",
                        Message = (message == "") ? "{{input.content}}" : message,
                        Intent = "",
                        NotHandled = (notHandled) ? notHandled : false,
                        Version = "1.0",
                        UserId = "{{contact.identity}}",
                        TimeStamp = "{{calendar.unixTimeMilliseconds}}",
                    }
                };
            cbRequest.Content.NotHandled = notHandled;

            return cbRequest;
        }

        public CustomAction GetAgentChatbaseCustomAction(string agentMessage,
            ChatbaseRequest cbRequest,
            string type = null,
            string message = null)
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
