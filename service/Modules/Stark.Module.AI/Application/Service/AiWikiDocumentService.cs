using SqlSugar;
using Stark.Module.AI.Backgrounds;
using Stark.Module.AI.Domain;
using Stark.Module.AI.Models.Enum;
using Stark.Module.AI.Models.Request;
using Stark.Starter.Core.Extensions;
using Stark.Starter.Web.Models;
using SystemModule.Application;

namespace Stark.Module.AI.Application.Service;

/// <summary>
/// 知识库文档服务
/// </summary>
public class AiWikiDocumentService : BaseService
{
    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="request"></param>
    public async Task AddWikiDocument(WikiDocumentAddRequest request)
    {
        var document = new AiWikiDocument()
        {
            WikiId = request.WikiId,
            FileName = request.FileName,
            Path = request.Path,
            Text = request.Text,
            Type = request.Type,
            Status = AiKDocumentStatus.Logging
        };

        if (document.FileName.IsNullOrWhiteSpace())
        {
            document.FileName = document.Type.GetDescriptionString();
        }

        await _dbContext.AiWikiDocument.AddAsync(document);
        await _dbContext.SaveChangesAsync();
        await QuantizeBackgroundService.AddTaskAsync(document);
    }

    /// <summary>
    /// 修改文档导入状态
    /// </summary>
    /// <param name="wikiDetail"></param>
    /// <param name="status"></param>
    public async Task UpdateState(AiWikiDocument wikiDetail, AiKDocumentStatus status)
    {
        wikiDetail.Status = status;
        _dbContext.AiWikiDocument.Update(wikiDetail);
        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// 知识库文档分页列表
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    public async Task<PageResult<AiWikiDocument>> PageAsync(WikiDocumentPageRequest dto)
    {
        var total = new RefAsync<int>(0);
        var listEntity = await _baseQuery.Db.Queryable<AiWikiDocument>()
            .Where(x => x.WikiId == dto.WikiId)
            .WhereIF(!dto.FileName.IsNullOrWhiteSpace(),x => x.FileName.Contains(dto.FileName))
            .WhereIF(dto.Type != null,x => x.Type == dto.Type)
            .WhereIF(dto.Status != null,x => x.Status == dto.Status)
            .ToPageListAsync(dto.PageNo, dto.PageSize, total);

        var result = new PageResult<AiWikiDocument>
        {
            TotalCount = total,
            Rows = listEntity
        };
        return result;
    }
}