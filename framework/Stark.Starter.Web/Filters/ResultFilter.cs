using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Stark.Starter.Core.Const;
using Stark.Starter.Web.Attributes;
using Stark.Starter.Web.Logger;
using Stark.Starter.Web.Models;
using Volo.Abp;

namespace Stark.Starter.Web.Filters;

/// <summary>
/// 结果过滤器
/// </summary>
public class ResultFilter : IAsyncResultFilter, IAsyncExceptionFilter
{
    /// <summary>
    /// 对返回结果进行发封装
    /// </summary>
    /// <param name="context"></param>
    /// <param name="next"></param>
    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        var controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
        var actionWrapper = controllerActionDescriptor?.MethodInfo.GetCustomAttributes(typeof(DontWrapResultAttribute), false).FirstOrDefault();
        var controllerWrapper = controllerActionDescriptor?.ControllerTypeInfo.GetCustomAttributes(typeof(DontWrapResultAttribute), false).FirstOrDefault();
        //如果包含DontWrapResultAttribute则说明不需要对返回结果进行包装，直接返回原始值
        if (actionWrapper != null || controllerWrapper != null)
        {
            return;
        }
        
        var result = context.Result;
        
        if (result is ObjectResult data)
        {
            if (data.Value is ApiResult)
            {
                context.Result = new OkObjectResult(context.Result);
            }
            else
            {
                context.Result = new OkObjectResult(ApiResult<object?>.Success(data.Value));
            }
        }
        else if (result is EmptyResult)
        {
            context.Result = new JsonResult(ApiResult.Success());
        }

        await next();
    }

    /// <summary>
    /// 发生错误时
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public Task OnExceptionAsync(ExceptionContext context)
    {
        //判断异常是否已经处理
        if (!context.ExceptionHandled)
        {
            var result = ApiResult.Failed(context.Exception.Message);
            
            if (context.Exception is not UserFriendlyException)
            {
                var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<ResultFilter>>();

                // 获取 HttpContext 和 HttpRequest 对象
                var httpContext = context.HttpContext;
                var httpRequest = httpContext.Request;

                // 获取授权用户
                var user = httpContext.User;
                // 获取当前操作者
                string userName = "", loginName = "";
                foreach (var item in user.Claims)
                {
                    switch (item.Type)
                    {
                        case ClaimConst.UserName:
                            userName = item.Value;
                            break;
                        case ClaimConst.LoginName:
                            loginName = item.Value;
                            break;
                    }
                }

                // 设置日志上下文
                logger.BeginScope(new LogInfo
                {
                    RequestUrl = httpRequest.Path,
                    LogDateTime = DateTime.Now,
                    UserName = userName,
                    LoginName = loginName,
                    Exception = context.Exception
                });

                logger.LogError(context.Exception.StackTrace, context.Exception.Message);
                Console.WriteLine(context.Exception);
            }

          
            context.Result = new JsonResult(result);
            context.ExceptionHandled = true;
        }

        return Task.CompletedTask;
    }
}