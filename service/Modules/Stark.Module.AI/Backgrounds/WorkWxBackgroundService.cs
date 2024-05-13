using System.Threading.Channels;
using Stark.Module.AI.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Stark.Module.AI.Application.Sk;
using Stark.Module.AI.Models.Request;
using Stark.Module.AI.Options;
using Stark.Starter.Work.Weixin.Client;
using Stark.Starter.Work.Weixin.Event.Messages;
using Stark.Starter.Work.Weixin.Models;
using Stark.Starter.Work.Weixin.Models.Request;

namespace Stark.Module.AI.Backgrounds;

public class WorkWxBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// 当前任务数量
    /// </summary>
    private static int CurrentTask = 0;

    private readonly WorkWxConfig _workWxConfig;

    /// <summary>
    /// 最大量化任务数量
    /// </summary>
    public static int MaxTask = 3;

    private static readonly Channel<TextMessage> TextMessageChannel = Channel.CreateBounded<TextMessage>(
        new BoundedChannelOptions(1000)
        {
            SingleReader = true,
            SingleWriter = false
        });

    /// <summary>
    /// 构造
    /// </summary>
    public WorkWxBackgroundService(IServiceProvider serviceProvider, IOptions<WorkWxConfig> workWxOptions)
    {
        _serviceProvider = serviceProvider;
        _workWxConfig = workWxOptions.Value;
    }

    /// <summary>
    /// 执行
    /// </summary>
    /// <param name="stoppingToken"></param>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (MaxTask < 0)
        {
            MaxTask = 1;
        }

        for (int i = 0; i < MaxTask; i++)
        {
            await Task.Factory.StartNew(state => HandlerAsync(), stoppingToken,
                TaskCreationOptions.LongRunning);
        }
    }

    /// <summary>
    /// 添加量化任务
    /// </summary>
    /// <param name="document"></param>
    public static async Task AddTaskAsync(TextMessage document)
    {
        await TextMessageChannel.Writer.WriteAsync(document);
    }

    private async Task HandlerAsync()
    {
        using var asyncServiceScope = _serviceProvider.CreateScope();
        while (await TextMessageChannel.Reader.WaitToReadAsync())
        {
            Interlocked.Increment(ref CurrentTask);
            var wikiDetail = await TextMessageChannel.Reader.ReadAsync();
            await HandlerAsync(wikiDetail, asyncServiceScope.ServiceProvider);
            Interlocked.Decrement(ref CurrentTask);
        }
    }

    /// <summary>
    /// 处理消息
    /// </summary>
    /// <param name="message"></param>
    private async ValueTask HandlerAsync(TextMessage message, IServiceProvider service)
    {
        var chatService = service.GetRequiredService<IChatService>();
        var messageClient = service.GetRequiredService<IMessageClient>();
        try
        {
            var chatResult = await chatService.ChatAsync(null, new ChatRequest()
            {
                WikiId = WorkAiOption.WikiId,
                Stream = false,
                Context = message.Context
            });

            await messageClient.SendMessageAsync(new SendMessageRequest()
            {
                msgtype = "text",
                touser = message.FromUserName,
                agentid = _workWxConfig.AgentId,
                text = new MessageText()
                {
                    content = chatResult.Content
                }
            });
        }
        catch (Exception e)
        {
        }
    }
}