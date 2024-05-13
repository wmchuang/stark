using Stark.Module.AI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Stark.Module.AI.Application.Sk;
using Stark.Module.AI.Models.Request;
using Stark.Starter.Web.Attributes;

namespace Stark.Module.AI.Controllers;

/// <summary>
/// AI
/// </summary>
public class AiController : BaseController
{
    private readonly IChatService _chatService;

    public AiController(IChatService chatService)
    {
        _chatService = chatService;
    }

    /// <summary>
    /// 对话
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [DontWrapResult]
    public async Task Chat(ChatRequest request)
    {
        var chatResult = await _chatService.ChatAsync(HttpContext, request);
        if (chatResult != null)
        {
            HttpContext.Response.ContentType = "application/json";
            await HttpContext.Response.WriteAsync(JsonConvert.SerializeObject(chatResult.Content));
            await HttpContext.Response.CompleteAsync();
        }
    }
}