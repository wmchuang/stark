using Stark.Module.AI.Models.Result;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SqlSugar;
using Stark.Module.AI.Domain;
using Stark.Module.AI.Models.Request;
using Stark.Starter.Web.Models;
using SystemModule.Application;

namespace Stark.Module.AI.Application.Service;

/// <summary>
/// 智能体服务
/// </summary>
public class AiBotService : BaseService
{
    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="request"></param>
    public async Task AddAsync(BotAddRequest request)
    {
        await _dbContext.AiBot.AddAsync(new AiBot()
        {
            Name = request.Name,
            Description = request.Description,
            ChatModelId = request.ChatModelId,
            Avatar = request.Avatar,
            Prompting = request.Prompting,
            Temperature = request.Temperature,
            Opening = request.Opening,
            StartPrologues = JsonConvert.SerializeObject(request.StartPrologues),
            WikiIds = JsonConvert.SerializeObject(request.WikiIds)
        });
        await _dbContext.SaveChangesAsync();
    }
    
    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="request"></param>
    public async Task UpdateAsync(BotUpdateRequest request)
    {
        var bot = await _dbContext.AiBot.FirstAsync(x => x.Id == request.Id);
        bot.Name = request.Name;
        bot.Description = request.Description;
        bot.ChatModelId = request.ChatModelId;
        bot.Avatar = request.Avatar;
        bot.Prompting = request.Prompting;
        bot.Temperature = request.Temperature;
        bot.Opening =request.Opening;
        bot.StartPrologues = JsonConvert.SerializeObject(request.StartPrologues);
        bot.WikiIds = JsonConvert.SerializeObject(request.WikiIds);
        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// 分页列表
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    public async Task<PageResult<AiBot>> PageAsync(PageRequest dto)
    {
        var total = new RefAsync<int>(0);
        var listEntity = await _baseQuery.Db.Queryable<AiBot>()
            .ToPageListAsync(dto.PageNo, dto.PageSize, total);

        var result = new PageResult<AiBot>
        {
            TotalCount = total,
            Rows = listEntity
        };
        return result;
    }

    /// <summary>
    /// 详情
    /// </summary>
    /// <param name="botId"></param>
    /// <returns></returns>
    public async Task<AiBot> DetailAsync(string botId)
    {
        var bot = await _baseQuery.Db.Queryable<AiBot>()
            .FirstAsync(x => x.Id == botId);
       
        return bot;
    }
}