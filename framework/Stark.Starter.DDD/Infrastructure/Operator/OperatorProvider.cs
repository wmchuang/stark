using Microsoft.AspNetCore.Http;
using Stark.Starter.Core.Const;
using Volo.Abp.DependencyInjection;

namespace Stark.Starter.DDD.Infrastructure.Operator;

/// <summary>
/// 审计操作人信息提供者
/// </summary>
public class OperatorProvider : IOperatorProvider, IScopedDependency
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    private Operator? _operator;

    public OperatorProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Operator GetOperator()
    {
        if (_operator != null)
        {
            return _operator;
        }

        if (_httpContextAccessor.HttpContext != null)
        {
            var currentUser = _httpContextAccessor.HttpContext.User;
            var isAuthenticated = currentUser.Identity?.IsAuthenticated ?? false;
            if (isAuthenticated)
            {
                var operatorId = currentUser.Claims.FirstOrDefault(x => x.Type == ClaimConst.UserId)?.Value ?? string.Empty;
                var operatorName = currentUser.Claims.FirstOrDefault(x => x.Type ==  ClaimConst.UserName)?.Value ?? string.Empty;
                _operator = new Operator
                {
                    OperatorId = operatorId,
                    OperatorName = operatorName
                };
                return _operator;
            }
        }

        _operator = new Operator();
        return _operator;
    }
}