namespace Stark.Starter.Work.Weixin.Models;

public class WorkWxTokenCached
{
    public const string Cache_key = "WECHATTOKEN:{0}";


    /// <summary>
    /// 微信AccessToken
    /// </summary>
    public string AccessToken { get; set; }

    /// <summary>
    /// 微信jsapi_ticket
    /// </summary>
    public string JsApiTicket { get; set; }

    /// <summary>
    /// 有效期
    /// </summary>
    public DateTime ExpireTime { get; set; }

    /// <summary>
    /// 获取缓存Key
    /// </summary>
    /// <param name="appCode"></param>
    /// <returns></returns>
    public static string GetCachedKey(string appCode)
    {
        return string.Format(Cache_key, appCode);
    }
}