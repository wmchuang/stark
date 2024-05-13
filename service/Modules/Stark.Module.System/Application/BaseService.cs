using MapsterMapper;
using Stark.Module.System.Infrastructure;
using Stark.Starter.DDD.Infrastructure.Operator;
using Stark.Starter.DDD.Infrastructure.SqlSugar;
using Volo.Abp.DependencyInjection;

namespace Stark.Module.System.Application;

public abstract class BaseService : ITransientDependency
{
    public IAbpLazyServiceProvider LazyServiceProvider { get; set; } = default!;
    
    public SystemDbContext _dbContext => LazyServiceProvider.LazyGetRequiredService<SystemDbContext>();
    
    public IBaseQuery _baseQuery => LazyServiceProvider.LazyGetRequiredService<IBaseQuery>();
    
    public IMapper _mapper => LazyServiceProvider.LazyGetRequiredService<IMapper>();
    
    public IOperatorProvider _operator => LazyServiceProvider.LazyGetRequiredService<IOperatorProvider>();
}