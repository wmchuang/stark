using Microsoft.AspNetCore.Mvc;
using Stark.Starter.Job;

namespace Stark.Module.Test.Controllers;

/// <summary>
/// job
/// </summary>
public class JobController : BaseController
{
    private readonly JobManager _jobManager;

    public JobController(JobManager jobManager)
    {
        _jobManager = jobManager;
    }
    
    /// <summary>
    /// 获取所有
    /// </summary>
    [HttpGet]
    public async Task GetJobListAsync()
    {
        await _jobManager.GetJobListAsync();
    }
}