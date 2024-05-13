using System.ComponentModel;
using System.Reflection;

namespace Stark.Starter.Core.Extensions;

public static class EnumExtensions
{
    /// <summary>
    /// 获取枚举描述
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string GetDescriptionString(this System.Enum value)
    {
        var fieldInfo = value.GetType().GetField(value.ToString());
        return fieldInfo != null ? GetDescription(fieldInfo) : string.Empty;
    }

    /// <summary>
    /// 获取成员信息的 Description
    /// </summary>
    /// <param name="fieldInfo">成员信息</param>
    /// <returns>Description</returns>
    private static string GetDescription(FieldInfo fieldInfo)
    {
        var customAttribute = Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute));
        return customAttribute == null ? string.Empty : ((DescriptionAttribute)customAttribute).Description;
    }
}