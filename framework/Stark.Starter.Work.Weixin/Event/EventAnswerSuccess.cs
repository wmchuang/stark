namespace Stark.Starter.Work.Weixin.Event
{
    /// <summary>
    /// 成功的事件应答
    /// </summary>
    public class EventAnswerSuccess : IEventAnswer
    {
        public string FormatString()
        {
            return "success";
        }
    }
}