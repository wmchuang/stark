using SqlSugar;
using Stark.Module.System.Domain;
using Stark.Module.System.Models.Requests.SysLogVisit;
using Stark.Starter.Web.Models;

namespace Stark.Module.System.Application.Services;

/// <summary>
/// 访问日志
/// </summary>
public class SysLogVisitService : BaseService
{
    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="sysLogVisit"></param>
    public async Task AddAsync(SysLogVisit sysLogVisit)
    {
        await _dbContext.SysLogVisit.AddAsync(sysLogVisit);
        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// 分页列表
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    public async Task<PageResult<SysLogVisit>> PageAsync(LogVisitPageRequest dto)
    {
        var total = new RefAsync<int>(0);
        var rows = await _baseQuery.Db.Queryable<SysLogVisit>()
            .WhereIF(dto.StartTime.HasValue, x => x.CreateTime >= dto.StartTime)
            .WhereIF(dto.EndTime.HasValue, x => x.CreateTime <= dto.EndTime)
            .OrderByDescending(x => x.CreateTime)
            .ToPageListAsync(dto.PageNo, dto.PageSize, total);

        var result = new PageResult<SysLogVisit>
        {
            TotalCount = total,
            Rows = rows,
        };
        return result;
    }
}