using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Stark.Starter.Work.Weixin.Models.Result;

public class GetAccessTokenResult: BaseWeChatResult
{
    /// <summary>
    /// token
    /// </summary>
    [JsonProperty("access_token")]
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; }

    /// <summary>
    /// 有效期
    /// </summary>
    [JsonProperty("expires_in")]
    [JsonPropertyName("expires_in")]
    public int Expires { get; set; }
}