using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Stark.Module.Test.Options;

namespace Stark.Module.Test.Controllers;

public class ConfigController : BaseController
{
    private readonly IConfiguration _configuration;
    private readonly JobConfig _jobConfig;

    public ConfigController(IConfiguration configuration,IOptionsMonitor<JobConfig> jobConfig)
    {
        _configuration = configuration;
        _jobConfig = jobConfig.CurrentValue;
    }
    
    /// <summary>
    /// 获取所有
    /// </summary>
    [HttpGet]
    public async Task<string> TestConfigAsync()
    {
        return _jobConfig.Job2Cron;
    }
}