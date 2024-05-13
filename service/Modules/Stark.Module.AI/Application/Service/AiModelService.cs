using Microsoft.EntityFrameworkCore;
using SqlSugar;
using Stark.Module.AI.Domain;
using Stark.Module.AI.Models.Enum;
using Stark.Module.AI.Models.Request;
using Stark.Starter.Web.Models;
using SystemModule.Application;

namespace Stark.Module.AI.Application.Service;

/// <summary>
/// 模型服务
/// </summary>
public class AiModelService : BaseService
{
    /// <summary>
    /// 添加模型
    /// </summary>
    /// <param name="request"></param>
    public async Task AddModelAsync(ModelAddRequest request)
    {
        await _dbContext.AiModels.AddAsync(new AiModel()
        {
            Description = request.Description,
            Type = request.Type,
            ModelType = request.ModelType,
            EndPoint = request.EndPoint,
            ModelName = request.ModelName,
            ModelKey = request.ModelKey
        });

        await _dbContext.SaveChangesAsync();
    }
    
    /// <summary>
    /// 复制模型
    /// </summary>
    /// <param name="modelId"></param>
    public async Task CopyModelAsync(string modelId)
    {
        var model = await _dbContext.AiModels.FirstAsync(x => x.Id == modelId);
        await _dbContext.AiModels.AddAsync(new AiModel()
        {
            Description = model.Description + "-[复制]",
            Type = model.Type,
            ModelType = model.ModelType,
            EndPoint = model.EndPoint,
            ModelName = model.ModelName,
            ModelKey = model.ModelKey
        });

        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// 修改模型
    /// </summary>
    /// <param name="request"></param>
    public async Task UpdateModelAsync(ModelUpdateRequest request)
    {
        var entity = await _dbContext.AiModels.FirstAsync(x => x.Id == request.Id);
        entity.Description = request.Description;
        entity.Type = request.Type;
        entity.ModelType = request.ModelType;
        entity.EndPoint = request.EndPoint;
        entity.ModelName = request.ModelName;
        entity.ModelKey = request.ModelKey;

        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// 分页列表
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<PageResult<AiModel>> PageAsync(ModelPageRequest request)
    {
        var total = new RefAsync<int>(0);
        var rows = await _baseQuery.Db.Queryable<AiModel>()
            .WhereIF(request.Type != null, x => x.Type == request.Type)
            .WhereIF(request.ModelType != null, x => x.ModelType == request.ModelType)
            .ToPageListAsync(request.PageNo, request.PageSize, total);

        var result = new PageResult<AiModel>
        {
            TotalCount = total,
            Rows = rows
        };
        return result;
    }

    /// <summary>
    /// 列表
    /// </summary>
    /// <param name="modelType"></param>
    /// <returns></returns>
    public async Task<List<AiModel>> ListAsync(AiModelTypeEnum? modelType)
    {
        var rows = await _baseQuery.Db.Queryable<AiModel>()
            .WhereIF(modelType != null, x => x.ModelType == modelType)
            .ToListAsync();

        return rows;
    }

    /// <summary>
    /// 获取单个详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<AiModel> GetModelAsync(string id)
    {
        return await _dbContext.AiModels.FirstAsync(x => x.Id == id);
    }
}