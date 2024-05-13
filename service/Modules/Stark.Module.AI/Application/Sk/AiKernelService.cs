using Microsoft.SemanticKernel;
using Stark.Module.AI.Application.Service;
using Stark.Module.AI.Domain;

namespace Stark.Module.AI.Application.Sk;

public class AiKernelService : IAiKernelService
{
    private readonly AiModelService _modelService;

    public AiKernelService(AiModelService modelService)
    {
        _modelService = modelService;
    }

    public async Task<Kernel> GetKernel(AiBot bot)
    {
        var model = await _modelService.GetModelAsync(bot.ChatModelId);
        var kernel = Kernel.CreateBuilder().AddChatCompletion(model: model);
        return kernel.Build();
    }
}