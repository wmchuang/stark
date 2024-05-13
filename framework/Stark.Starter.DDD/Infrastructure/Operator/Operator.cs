namespace Stark.Starter.DDD.Infrastructure.Operator;

/// <summary>
/// 操作人信息
/// </summary>
public class Operator
{
    
    public Operator()
    {
        OperatorId = "";
        OperatorName = "System";
    }

    /// <summary>
    /// 操作人Id
    /// </summary>
    public string OperatorId { get; set; }

    /// <summary>
    /// 操作人名称
    /// </summary>
    public string OperatorName { get; set; }
}