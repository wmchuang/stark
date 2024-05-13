using Microsoft.AspNetCore.Http;
using Microsoft.SemanticKernel;
using Stark.Module.AI.Models.Request;
using Volo.Abp.DependencyInjection;

namespace Stark.Module.AI.Application.Sk;

public interface IChatService : ITransientDependency
{
    /// <summary>
    /// 对话
    /// </summary>
    /// <param name="httpContext"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    public Task<ChatMessageContent?> ChatAsync(HttpContext httpContext, ChatRequest request);
}