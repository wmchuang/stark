namespace Stark.Starter.Web.Attributes;

/// <summary>
/// 用于判断Web Api不需要包装返回的结果
/// </summary>
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class DontWrapResultAttribute : Attribute
{
}
