namespace Stark.Starter.Work.Weixin.Event
{
    /// <summary>
    /// 空的事件应答
    /// </summary>
    public class EventAnswerEmpty : IEventAnswer
    {
        public string FormatString()
        {
            return string.Empty;
        }
    }
}