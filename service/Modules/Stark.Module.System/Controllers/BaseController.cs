using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Stark.Module.System.Controllers;

/// <summary>
/// 基本控制器
/// </summary>
[ApiController]
[Route("api/sys/[controller]/[action]")]
[Authorize]
[ApiExplorerSettings(GroupName = "StarkSystemModule")]
public class BaseController : ControllerBase
{
    
}