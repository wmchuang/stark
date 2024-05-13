using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Stark.Starter.Work.Weixin.Models.Result;

public class BaseWeChatResult
{
    /// <summary>
    /// 错误码
    /// </summary>
    [JsonProperty("errcode")]
    [JsonPropertyName("errcode")]
    public int ErrorCode { get; set; }

    /// <summary>
    /// 错误信息
    /// </summary>
    [JsonProperty("errmsg")]
    [JsonPropertyName("errmsg")]
    public string ErrorMessage { get; set; }
}