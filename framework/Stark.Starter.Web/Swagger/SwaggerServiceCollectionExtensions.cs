using System.Reflection;
using System.Xml.Linq;
using System.Xml.XPath;
using IGeekFan.AspNetCore.Knife4jUI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Stark.Starter.Web.Swagger;

public static class SwaggerServiceCollectionExtensions
{
    public static void AddSwaggerConfig(this IServiceCollection services,Action<SwaggerGenOptions> swaggerOptions)
    {
        services.AddSwaggerGen(options =>
        {
            options.SchemaFilter<EnumSchemaFilter>();
            swaggerOptions?.Invoke(options);

            var ioc = services.BuildServiceProvider();
            var descriptionProvider = ioc.GetRequiredService<IApiDescriptionGroupCollectionProvider>();
            
            // Items 是根据 ApiExplorerSettings.GroupName 进行分组的
            foreach (var description in descriptionProvider.ApiDescriptionGroups.Items)
            {
                var group = description.GroupName ?? "All";
                // 如果 Controller 没有配置分组，则放到默认分组中
                options.SwaggerDoc(group, new OpenApiInfo
                {
                    Version = "v1.0",
                    Title = description.GroupName,
                });
            }

            options.DocInclusionPredicate((docName, apiDesc) =>
            {
                if (docName.StartsWith("All"))
                {
                    return true;
                }

                if (!apiDesc.TryGetMethodInfo(out MethodInfo methodInfo))
                {
                    return false;
                }

                var versions = methodInfo.DeclaringType.GetCustomAttributes(true)
                    .OfType<ApiExplorerSettingsAttribute>()
                    .Select(x => x.GroupName);
                return versions.Any(x => x.ToString() == docName);
            });
            
            
          
            var directoryInfo = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            var fileInfos = directoryInfo.GetFiles();
            var xmlFiles = fileInfos.Where(x => x.Name.EndsWith(".xml"));
            foreach (var file in xmlFiles)
            {
                var xmlDoc = XDocument.Load(file.FullName);

                options.IncludeXmlComments(() => new XPathDocument(xmlDoc.CreateReader()), true);
            }
        });
    }

    /// <summary>
    /// 使用swagger
    /// </summary>
    /// <param name="app">IApplicationBuilder</param>
    public static void UserSwaggerUi(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseKnife4UI(c =>
        {
            var ioc = app.ApplicationServices;
             var descriptionProvider = ioc.GetRequiredService<IApiDescriptionGroupCollectionProvider>();
             // 配置页面显示和使用哪些位置的 swagger.json 文件
             foreach (var description in descriptionProvider.ApiDescriptionGroups.Items)
             {
                 var group = description.GroupName ?? "All";
                 c.SwaggerEndpoint($"/swagger/{group}/swagger.json", group);
             }
           
            c.RoutePrefix = "";
            c.DocExpansion(DocExpansion.None);
            c.DocumentTitle = "Stark Api";
            c.EnableFilter();
        });
    }
}