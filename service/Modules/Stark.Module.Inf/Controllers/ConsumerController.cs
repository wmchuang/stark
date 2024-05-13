using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;

namespace Stark.Module.Inf.Controllers;

public class ConsumerController : Controller
{
    [NonAction]
    [CapSubscribe("test.show.time")]
    public void ReceiveMessage(DateTime time)
    {
        Console.WriteLine("message time is:" + time);
    }
}