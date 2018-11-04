using BuilderUtils.Models;

namespace BuilderUtils.Extensions
{
    public interface IChatbaseExtension
    {
        CBBoxContent EditChatbaseProperties();
        ChatbaseRequest GetAgentBodyRequest(CBBoxContent bodyContentModel, ChatbaseRequest cbRequest = null, string message = "", bool notHandled = false, string intent = "");
        CustomAction GetAgentChatbaseCustomAction(ChatbaseRequest cbRequest, string agentMessage = "", string type = null, string message = null);
        ChatbaseRequest GetChatbaseBodyRequest(CBBoxContent bodyContentModel, ChatbaseRequest cbRequest = null, string type = "", string message = "", bool notHandled = false, string intent = "");
        CustomAction GetUserChatbaseCustomAction(ChatbaseRequest cbRequest);
    }
}