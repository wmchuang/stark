using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Stark.Starter.Web.Filters;

public class LoggerFilter : IAsyncActionFilter
{
    private readonly ILogger<LoggerFilter> _logger;

    public LoggerFilter(ILogger<LoggerFilter> logger)
    {
        _logger = logger;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    { 
        await next();
    }
}