using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Stark.Module.Inf.Controllers;

/// <summary>
/// 基本控制器
/// </summary>
[ApiController]
[Route("api/inf/[controller]/[action]")]
public class BaseController : ControllerBase
{
    
}