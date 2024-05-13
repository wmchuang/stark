using System.Linq.Expressions;
using System.Reflection;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Stark.Starter.DDD.Infrastructure.EFCore.Modeling;
using Stark.Starter.DDD.Domain.Entities;
using Stark.Starter.DDD.Infrastructure.Operator;
using Volo.Abp;
using Volo.Abp.DependencyInjection;

namespace Stark.Starter.DDD.Infrastructure.EFCore;

public class StarkDbContext<TDbContext> : DbContext where TDbContext : DbContext
{
    public StarkDbContext(DbContextOptions<TDbContext> options) : base(options)
    {
    }

    public IAbpLazyServiceProvider LazyServiceProvider { get; set; } = default!;

    public IOperatorProvider _operatorProvider => LazyServiceProvider.LazyGetRequiredService<IOperatorProvider>();
    public IMediator _mediator => LazyServiceProvider.LazyGetRequiredService<IMediator>();

    private static readonly MethodInfo ConfigureBasePropertiesMethodInfo
        = typeof(StarkDbContext<TDbContext>)
            .GetMethod(
                nameof(ConfigureBaseProperties),
                BindingFlags.Instance | BindingFlags.NonPublic
            )!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            ConfigureBasePropertiesMethodInfo
                .MakeGenericMethod(entityType.ClrType)
                .Invoke(this, new object[] { modelBuilder, entityType });
        }
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        //发布事件
        await DispatchDomainEventsAsync();

        BeforeSaveChange(ChangeTracker.Entries<AggregateRoot>().ToList());
        BeforeSaveChange(ChangeTracker.Entries<ISoftDelete>().ToList());

        var result = await base.SaveChangesAsync(cancellationToken);

        return result;
    }

    /// <summary>
    /// 写入数据库前,设置实体审计
    /// </summary>
    /// <param name="entities"></param>
    private void BeforeSaveChange(IReadOnlyCollection<EntityEntry<AggregateRoot>> entities)
    {
        if (!entities.Any()) return;

        var @operator = _operatorProvider.GetOperator();

        foreach (var e in entities)
        {
            switch (e.State)
            {
                case EntityState.Added:
                    if (string.IsNullOrWhiteSpace(e.Entity.CreateBy))
                        e.Entity.SetCreate(@operator.OperatorId, @operator.OperatorName);

                    break;
                case EntityState.Modified:
                    e.Entity.SetModifier(@operator.OperatorId, @operator.OperatorName);
                    break;
            }
        }
    }
    
    /// <summary>
    /// 写入数据库前,如果继承自软删除,则设置软删除
    /// </summary>
    /// <param name="entities"></param>
    private void BeforeSaveChange(IReadOnlyCollection<EntityEntry<ISoftDelete>> entities)
    {
        if (!entities.Any()) return;

        foreach (var e in entities)
        {
            switch (e.State)
            {
                case EntityState.Deleted:
                    // e.Reload();

                    ObjectHelper.TrySetProperty(e.Entity.As<ISoftDelete>(), x => x.IsDeleted, () => true);
                    e.State = EntityState.Modified;
                    break;
            }
        }
    }

    /// <summary>
    /// 发布领域事件
    /// </summary>
    private async Task DispatchDomainEventsAsync()
    {
        var domainEntities = ChangeTracker
            .Entries<AggregateRoot>()
            .Where(x => x.Entity.LocalDomainEvents.Any());

        var entityEntries = domainEntities as EntityEntry<AggregateRoot>[] ?? domainEntities.ToArray();
        var domainEvents = entityEntries
            .SelectMany(x => x.Entity.LocalDomainEvents)
            .ToList();

        entityEntries.ToList()
            .ForEach(entity => entity.Entity.ClearLocalEvents());

        foreach (var domainEvent in domainEvents)
            await _mediator.Publish(domainEvent);
    }

    /// <summary>
    /// 配置基础属性
    /// </summary>
    /// <param name="modelBuilder"></param>
    /// <param name="mutableEntityType"></param>
    /// <typeparam name="TEntity"></typeparam>
    protected virtual void ConfigureBaseProperties<TEntity>(ModelBuilder modelBuilder, IMutableEntityType mutableEntityType)
        where TEntity : class
    {
        if (mutableEntityType.IsOwned())
        {
            return;
        }

        if (!typeof(Entity).IsAssignableFrom(typeof(TEntity)))
        {
            return;
        }

        modelBuilder.Entity<TEntity>().ConfigureByConvention();

        ConfigureGlobalFilters<TEntity>(modelBuilder, mutableEntityType);
    }

    protected virtual void ConfigureGlobalFilters<TEntity>(ModelBuilder modelBuilder, IMutableEntityType mutableEntityType)
        where TEntity : class
    {
        Expression<Func<TEntity, bool>>? expression = null;

        //Todo 如果还要加多租户的过滤器，需要和当前的组合起来
        if (typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
        {
            expression = e => !EF.Property<bool>(e, "IsDeleted");
        }

        modelBuilder.Entity<TEntity>().HasQueryFilter(expression);
    }
}