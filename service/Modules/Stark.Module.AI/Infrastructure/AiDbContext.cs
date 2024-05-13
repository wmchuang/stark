using Microsoft.EntityFrameworkCore;
using Stark.Module.AI.Domain;
using Stark.Starter.DDD.Infrastructure.EFCore;

namespace Stark.Module.AI.Infrastructure;

public class AiDbContext : StarkDbContext<AiDbContext>
{
    public AiDbContext(DbContextOptions<AiDbContext> options) : base(options)
    {
    }

    /// <summary>
    /// 知识库
    /// </summary>
    public DbSet<AiWiki> AiWiki { get; set; }

    /// <summary>
    /// 知识库文档
    /// </summary>
    public DbSet<AiWikiDocument> AiWikiDocument { get; set; }

    /// <summary>
    ///智能体
    /// </summary>
    public DbSet<AiBot> AiBot { get; set; }

    /// <summary>
    ///模型
    /// </summary>
    public DbSet<AiModel> AiModels { get; set; }

    /// <summary>
    /// 模型构建
    /// </summary>
    /// <param name="modelBuilder"></param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //使用当前类型所在的程序集，将 从当前类型所在的程序集加载所有配置应用到模型构建器中所有的配置信息并应用到模型构建
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}