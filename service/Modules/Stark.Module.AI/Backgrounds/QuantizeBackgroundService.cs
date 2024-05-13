using System.Threading.Channels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.KernelMemory;
using Stark.Module.AI.Application.Service;
using Stark.Module.AI.Application.Sk;
using Stark.Module.AI.Domain;
using Stark.Module.AI.Models.Enum;
using Stark.Starter.Web.Hub;

namespace Stark.Module.AI.Backgrounds;

/// <summary>
/// 量化后台任务
/// </summary>
public sealed class QuantizeBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly INoticeService _noticeService;

    /// <summary>
    /// 当前任务数量
    /// </summary>
    private static int CurrentTask = 0;

    /// <summary>
    /// 最大量化任务数量
    /// </summary>
    public static int MaxTask = 3;

    private static readonly Channel<AiWikiDocument> channel = Channel.CreateBounded<AiWikiDocument>(
        new BoundedChannelOptions(1000)
        {
            SingleReader = true,
            SingleWriter = false
        });

    /// <summary>
    /// 构造
    /// </summary>
    public QuantizeBackgroundService(IServiceProvider serviceProvider, INoticeService noticeService)
    {
        _serviceProvider = serviceProvider;
        _noticeService = noticeService;
    }

    /// <summary>
    /// 执行
    /// </summary>
    /// <param name="stoppingToken"></param>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // 获取环境变量中的最大任务数量
        var QUANTIZE_MAX_TASK = Environment.GetEnvironmentVariable("QUANTIZE_MAX_TASK");
        if (!string.IsNullOrEmpty(QUANTIZE_MAX_TASK))
        {
            int.TryParse(QUANTIZE_MAX_TASK, out MaxTask);
        }

        if (MaxTask < 0)
        {
            MaxTask = 1;
        }

        // await LoadingWikiDetailAsync();

        for (int i = 0; i < MaxTask; i++)
        {
            await Task.Factory.StartNew(state => DocumentHandlerAsync(), stoppingToken,
                TaskCreationOptions.LongRunning);
        }
    }

    /// <summary>
    /// 添加量化任务
    /// </summary>
    /// <param name="document"></param>
    public static async Task AddTaskAsync(AiWikiDocument document)
    {
        await channel.Writer.WriteAsync(document);
    }

    private async Task DocumentHandlerAsync()
    {
        using var asyncServiceScope = _serviceProvider.CreateScope();
        while (await channel.Reader.WaitToReadAsync())
        {
            Interlocked.Increment(ref CurrentTask);
            var wikiDetail = await channel.Reader.ReadAsync();
            await HandlerAsync(wikiDetail, asyncServiceScope.ServiceProvider);
            Interlocked.Decrement(ref CurrentTask);
        }
    }

    /// <summary>
    /// 处理量化
    /// </summary>
    /// <param name="document"></param>
    private async ValueTask HandlerAsync(AiWikiDocument document, IServiceProvider service)
    {
        var kmService = service.GetRequiredService<IAiKernelMemoryService>();
        var wikiService = service.GetRequiredService<AiWikiService>();
        var documentService = service.GetRequiredService<AiWikiDocumentService>();
        try
        {
            Console.WriteLine($"开始量化：documentId:{document.Id}");

            var wiki = await wikiService.GetAsync(document.WikiId);

            var memory = await kmService.GetMemoryAsync(wiki);

            string result = string.Empty;

            var tag = new TagCollection()
            {
                {
                    "documentId", document.Id
                }
            };
            if (document.Type == AiKDocumentType.File)
            {
                result = await memory.ImportDocumentAsync(document.Path, document.Id, tags: tag, document.WikiId);
            }
            else if (document.Type == AiKDocumentType.WebPage)
            {
                result = await memory.ImportWebPageAsync(document.Path, document.Id, tags: tag, document.WikiId);
            }
            else if (document.Type == AiKDocumentType.Text)
            {
                await memory.ImportTextAsync(document.Text, document.Id, tags: tag, document.WikiId);
            }

            await documentService.UpdateState(document, AiKDocumentStatus.Success);
            Console.WriteLine($"量化成功：{document.Id} {result}");

            await _noticeService.SendToAllClientsAsync($"量化成功：{document.Id} {result}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"量化失败{document.Id}  {Environment.NewLine + e.Message}");
            await _noticeService.SendToAllClientsAsync($"量化失败{document.Id}  {Environment.NewLine + e.Message}");
            await documentService.UpdateState(document, AiKDocumentStatus.Fail);
        }
    }

    // private async Task LoadingWikiDetailAsync()
    // {
    //     using var asyncServiceScope = _serviceProvider.CreateScope();
    //
    //     var wikiRepository = asyncServiceScope.ServiceProvider.GetRequiredService<IWikiRepository>();
    //     var mapper = asyncServiceScope.ServiceProvider.GetRequiredService<IMapper>();
    //     foreach (var wikiDetail in await wikiRepository.GetFailedDetailsAsync())
    //     {
    //         await AddWikiDetailAsync(mapper.Map<QuantizeWikiDetail>(wikiDetail));
    //     }
    // }
}