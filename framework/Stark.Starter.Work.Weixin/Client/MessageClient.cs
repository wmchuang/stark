using Microsoft.Extensions.Logging;
using Stark.Starter.Core.Extensions;
using Stark.Starter.Work.Weixin.Models.Request;
using Stark.Starter.Work.Weixin.Models.Result;
using Volo.Abp;

namespace Stark.Starter.Work.Weixin.Client
{
    /// <summary>
    /// 发送应用消息
    /// </summary>
    public class MessageClient : IMessageClient
    {
        private readonly ILogger<MessageClient> _logger;
        private readonly IAccessTokenClient _accessTokenClient;
        private readonly HttpClient _client;

        public MessageClient(ILogger<MessageClient> logger, IAccessTokenClient accessTokenClient, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _accessTokenClient = accessTokenClient;
            _client = httpClientFactory.CreateClient();
        }

        public async Task<bool> SendMessageAsync(SendMessageRequest request)
        {
            var token = await _accessTokenClient.GetAccessTokenAsync();
            var url = $"https://qyapi.weixin.qq.com/cgi-bin/message/send?access_token={token.AccessToken}";
            var result = await _client.PostAsync<SendMessageRequest, BaseWeChatResult>(url, request);
            if (result != null && result.ErrorCode == 0)
                return true;
            _logger.LogError(result?.ErrorMessage ?? "发送应用消息失败");
            throw new UserFriendlyException(result?.ErrorMessage ?? "发送应用消息失败");
        }
    }
}