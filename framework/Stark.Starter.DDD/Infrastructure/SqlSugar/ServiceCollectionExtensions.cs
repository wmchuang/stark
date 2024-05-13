using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SqlSugar;

namespace Stark.Starter.DDD.Infrastructure.SqlSugar
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSqlSugar(this IServiceCollection services, Func<ConnectionConfig> configFunc)
        {
            if (configFunc == null)
            {
                throw new ArgumentNullException(nameof(configFunc), "调用 SqlSugar 配置时出错，未传入配置过程。");
            }

            var config = configFunc.Invoke();

            services.TryAddSingleton(new ConnectionConfig
            {
                ConnectionString = config.ConnectionString,
                DbType = config.DbType,
                InitKeyType = InitKeyType.Attribute,
                IsAutoCloseConnection = true
            });

            services.TryAddScoped<IBaseQuery, BaseQuery>();
            return services;
        }
    }
}