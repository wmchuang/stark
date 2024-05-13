using SqlSugar;
using Volo.Abp;

namespace Stark.Starter.DDD.Infrastructure.SqlSugar
{
    public class BaseQuery : IBaseQuery
    {
        public BaseQuery(ConnectionConfig connectionConfig)
        {
            Db = new SqlSugarClient(connectionConfig);

            //添加全局过滤器
            Db.QueryFilter.AddTableFilter<ISoftDelete>(x => x.IsDeleted == false);

#if DEBUG
            //打印SQL 
            Db.Aop.OnLogExecuting = (sql, pars) =>
            {
                Console.WriteLine($"###############SqlSugar#########################");
                Console.WriteLine(SqlProfiler.ParameterFormat(sql, pars));
            };

#endif
        }

        /// <summary>
        /// 用来处理事务多表查询和复杂的操作
        /// </summary>
        public ISqlSugarClient Db { get; }

        public IAdo Ado => Db.Ado;
    }
}