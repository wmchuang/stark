using Stark.Module.AI.Infrastructure;
using Stark.Starter.DDD.Infrastructure.Operator;
using Stark.Starter.DDD.Infrastructure.SqlSugar;
using Volo.Abp.DependencyInjection;

namespace SystemModule.Application;

public abstract class BaseService : ITransientDependency
{
    public IAbpLazyServiceProvider LazyServiceProvider { get; set; } = default!;

    public AiDbContext _dbContext => LazyServiceProvider.LazyGetRequiredService<AiDbContext>();

    public IBaseQuery _baseQuery => LazyServiceProvider.LazyGetRequiredService<IBaseQuery>();

    public IOperatorProvider _operator => LazyServiceProvider.LazyGetRequiredService<IOperatorProvider>();
}