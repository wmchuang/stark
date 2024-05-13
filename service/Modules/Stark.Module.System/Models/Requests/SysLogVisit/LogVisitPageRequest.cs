using Stark.Starter.Web.Models;

namespace Stark.Module.System.Models.Requests.SysLogVisit;

public class LogVisitPageRequest : PageRequest
{
    /// <summary>
    /// 开始时间
    /// </summary>
    public DateTime? StartTime { get; set; }

    /// <summary>
    /// 结束时间
    /// </summary>
    public DateTime? EndTime { get; set; }
}