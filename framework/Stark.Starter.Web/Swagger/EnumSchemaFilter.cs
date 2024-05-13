using System.Text;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Stark.Starter.Core.Extensions;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Stark.Starter.Web.Swagger;

/// <summary>
/// Swagger文档枚举字段显示枚举属性和枚举值,以及枚举描述
/// </summary>
public class EnumSchemaFilter : ISchemaFilter
{
    /// <summary>
    /// 实现接口
    /// </summary>
    /// <param name="model"></param>
    /// <param name="context"></param>
    public void Apply(OpenApiSchema model, SchemaFilterContext context)
    {
        if (context.Type.IsEnum)
        {
            model.Enum.Clear();
            model.Type = "string";
            model.Format = null;

            StringBuilder stringBuilder = new StringBuilder();
            Enum.GetNames(context.Type)
                .ToList()
                .ForEach(name =>
                {
                    Enum e = (Enum)Enum.Parse(context.Type, name);
                    var descrptionOrNull = e.GetDescriptionString();
                    model.Enum.Add(new OpenApiString(name));
                    stringBuilder.Append(
                        $"【枚举：{name}{(descrptionOrNull is null ? string.Empty : $"({descrptionOrNull})")}={Convert.ToInt64(Enum.Parse(context.Type, name))}】<br />");
                });
            model.Description = stringBuilder.ToString();
        }
    }
}