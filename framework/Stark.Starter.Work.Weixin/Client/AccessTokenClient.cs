using Microsoft.Extensions.Options;
using Stark.Starter.Redis;
using Stark.Starter.Core.Extensions;
using Stark.Starter.Work.Weixin.Models;
using Stark.Starter.Work.Weixin.Models.Result;

namespace Stark.Starter.Work.Weixin.Client
{
    public class AccessTokenClient : IAccessTokenClient
    {
        private readonly IRedisCache _redisCache;
        private readonly WorkWxConfig _wOptions;
        private readonly HttpClient _client;

        public AccessTokenClient(IOptions<WorkWxConfig> wOptions, IRedisCache redisCache, IHttpClientFactory httpClientFactory)
        {
            _redisCache = redisCache;
            _wOptions = wOptions.Value;
            _client = httpClientFactory.CreateClient();
        }

        /// <summary>
        /// get AccessToken
        /// </summary>
        /// <returns></returns>
        public async Task<WorkWxTokenCached> GetAccessTokenAsync()
        {
            var key = WorkWxTokenCached.GetCachedKey(_wOptions.AgentId.ToString());
            var workWxToken = await _redisCache.GetAsync<WorkWxTokenCached>(key);
            if (workWxToken == null)
            {
                var url =
                    $"https://qyapi.weixin.qq.com/cgi-bin/gettoken?corpid={_wOptions.CorpID}&corpsecret={_wOptions.Secret}";

                var result = await _client.GetAsync<GetAccessTokenResult>(url);
                workWxToken = new WorkWxTokenCached
                {
                    AccessToken = result.AccessToken,
                    ExpireTime = DateTime.Now.AddSeconds(result.Expires)
                };

                await _redisCache.SetAsync(key, workWxToken,result.Expires - 1800);
            }

            return workWxToken;
        }
    }
}