using SqlSugar;

namespace Stark.Starter.DDD.Infrastructure.SqlSugar
{
    public interface IBaseQuery 
    {
        IAdo Ado { get; }
        ISqlSugarClient Db { get; }
    }
}