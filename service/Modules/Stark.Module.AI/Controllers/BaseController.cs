using Microsoft.AspNetCore.Mvc;

namespace Stark.Module.AI.Controllers;

/// <summary>
/// 基本控制器
/// </summary>
[ApiController]
[Route("api/ai/[controller]/[action]")]
[ApiExplorerSettings(GroupName = "StarkAIModule")]
public class BaseController : ControllerBase
{
    
}