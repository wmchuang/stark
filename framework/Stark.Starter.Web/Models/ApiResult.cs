namespace Stark.Starter.Web.Models;

/// <summary>
/// Api结果
/// </summary>
public class ApiResult
{
    public ApiResult()
    {
        Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    }

    public bool Succeeded { get; set; }

    /// <summary>
    /// 信息
    /// </summary>
    public string Msg { get; set; }

    /// <summary>
    /// 时间戳
    /// </summary>
    public long Timestamp { get; set; }

    public static ApiResult Failed(string msg)
    {
        return new ApiResult
        {
            Succeeded = false,
            Msg = msg
        };
    }

    public static ApiResult Success()
    {
        return new ApiResult
        {
            Succeeded = true,
            Msg = "操作成功"
        };
    }
}

/// <summary>
/// Api结果
/// </summary>
/// <typeparam name="T"></typeparam>
public class ApiResult<T> : ApiResult
{
    /// <summary>
    /// 数据
    /// </summary>
    public T? Data { get; set; }

    public static ApiResult<T?> Success(T? data = default)
    {
        return new ApiResult<T?>
        {
            Succeeded = true,
            Data = data,
            Msg = "操作成功"
        };
    }
}