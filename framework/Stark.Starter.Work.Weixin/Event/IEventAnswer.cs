namespace Stark.Starter.Work.Weixin.Event
{
    /// <summary>
    /// 事件应答
    /// </summary>
    public interface IEventAnswer
    {
        /// <summary>
        /// 格式化的应答字符串
        /// </summary>
        /// <returns></returns>
        string FormatString();
    }
}