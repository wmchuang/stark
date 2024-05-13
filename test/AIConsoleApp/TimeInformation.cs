using Microsoft.SemanticKernel;
using System.ComponentModel;

namespace AIModule;

public class TimeInformation
{
    [KernelFunction]
    [Description("当前时间")]
    public string GetCurrentUtcTime() => DateTime.UtcNow.ToString("R");
}