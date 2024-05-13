using Microsoft.SemanticKernel;
using Stark.Module.AI.Domain;
using Volo.Abp.DependencyInjection;

namespace Stark.Module.AI.Application.Sk;

/// <summary>
/// 
/// </summary>
public interface IAiKernelService : ITransientDependency
{
    /// <summary>
    /// 获取kennel
    /// </summary>
    /// <returns></returns>
    public Task<Kernel> GetKernel(AiBot bot);
}