using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Stark.Starter.Web.Models;

namespace Stark.Starter.Web.Filters;

/// <summary>
/// 模型验证
/// </summary>
public class DataValidationFilter : IAsyncActionFilter
{
    /// <summary>
    /// 执行动作方法前后进行拦截和处理
    /// </summary>
    /// <param name="context"></param>
    /// <param name="next"></param>
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.ModelState.IsValid)
        {
            var errors = context.ModelState.Where(x => x.Value != null).SelectMany(x => x.Value.Errors).Select(x => x.ErrorMessage).ToList();
            var result = ApiResult.Failed(string.Join("|", errors));

            context.Result = new JsonResult(result);

            return;
        }

        await next();
    }
}