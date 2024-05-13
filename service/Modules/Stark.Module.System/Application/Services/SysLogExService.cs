using SqlSugar;
using Stark.Module.System.Domain;
using Stark.Module.System.Models.Requests.SysLogVisit;
using Stark.Starter.Web.Models;

namespace Stark.Module.System.Application.Services;

public class SysLogExService : BaseService
{
    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="sysLogEx"></param>
    public async Task AddAsync(SysLogEx sysLogEx)
    {
        await _dbContext.SysLogEx.AddAsync(sysLogEx);
        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// 分页列表
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    public async Task<PageResult<SysLogEx>> PageAsync(LogVisitPageRequest dto)
    {
        var total = new RefAsync<int>(0);
        var rows = await _baseQuery.Db.Queryable<SysLogEx>()
            .WhereIF(dto.StartTime.HasValue, x => x.CreateTime >= dto.StartTime)
            .WhereIF(dto.EndTime.HasValue, x => x.CreateTime <= dto.EndTime)
            .OrderByDescending(x => x.CreateTime)
            .ToPageListAsync(dto.PageNo, dto.PageSize, total);

        var result = new PageResult<SysLogEx>
        {
            TotalCount = total,
            Rows = rows,
        };
        return result;
    }
}