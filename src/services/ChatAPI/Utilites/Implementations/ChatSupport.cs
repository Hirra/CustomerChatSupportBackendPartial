using ChatAPI.Models;
using ChatAPI.Utilites.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace ChatAPI.Utilites.Implementations
{
    public class ChatSupport :IChatSupport
    {
        private readonly IHttpClientWrapper httpClientWrapper;
        private readonly IOptions<ChatSupportApiSettings> chatSupportManagement; 
        private readonly ILogger<ChatSupport> logger;

        public ChatSupport(IHttpClientWrapper httpClientWrapper, IOptions<ChatSupportApiSettings> chatSupportManagement, ILogger<ChatSupport> logger)
        {
            this.httpClientWrapper = httpClientWrapper;
            this.chatSupportManagement = chatSupportManagement; 
            this.logger = logger;
        }
        public async Task<bool> RefuseNewChats()
        {
            try
            {
                var url = GetChatManagerUrl(); 
                var response = await this.httpClientWrapper.GetAsync(url);
                if (response.IsSuccessStatusCode)
                    return false;
            }
            catch (Exception exception)
            {
                this.logger.LogDebug("Error while making chat support call");
                this.logger.LogDebug(exception.Message + "\\n" + exception.StackTrace);
                return true;
            }

            return true;
        }

        private string GetChatManagerUrl()
        {
            return this.chatSupportManagement.Value.Protocol + "://" + this.chatSupportManagement.Value.HostName + ":" + this.chatSupportManagement.Value.Port + "/" + this.chatSupportManagement.Value.Route + "/CanAcceptNewChats";
        }
    }
}
