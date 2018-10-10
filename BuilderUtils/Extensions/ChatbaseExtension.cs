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
        private readonly Random rand = new Random();
        public JObject chatbaseRequest;

        public string GetChatbaseBodyRequest(ChatbaseRequest cbRequest, string type, string message)
        {
            cbRequest.Content.Type = type;
            cbRequest.Content.Message = message;

            return JsonConvert.SerializeObject(cbRequest.Content);
        }

        public CustomAction GetAgentChatbaseCustomAction(string agentMessage, 
            ChatbaseRequest cbRequest,
            string bodyRequest = null, 
            string type = null, 
            string message = null)
        {

            if (bodyRequest.Equals(null))
            {
                bodyRequest = GetChatbaseBodyRequest(cbRequest, type, message);
            }

            var action = new CustomAction()
            {
                Type = "ProcessHttp",
                Title = "Chatbase Event Tracking - Agent",
                Invalid = false,
                Settings = new CustomActionSettings()
                {
                    Headers = { },
                    Method = "POST",
                    Body = JsonConvert.SerializeObject(bodyRequest),
                    Uri = "{{config.chatbaseUrl}}"
                }
            };

            return action;
        }
    }
}
