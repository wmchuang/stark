namespace Stark.Starter.DDD.Infrastructure.Operator;

/// <summary>
/// 审计操作人信息提供者
/// </summary>
public interface IOperatorProvider
{
    public Operator GetOperator();
}