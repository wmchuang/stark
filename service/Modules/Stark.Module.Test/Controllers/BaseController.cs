using Microsoft.AspNetCore.Mvc;

namespace Stark.Module.Test.Controllers;

/// <summary>
/// 基本控制器
/// </summary>
[ApiController]
[ApiExplorerSettings(GroupName = "Stark.Module.Test")]
[Route("api/test/[controller]/[action]")]
public class BaseController : ControllerBase
{
    
}