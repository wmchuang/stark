using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;

namespace Stark.Module.Test.Controllers;

public class PublishController : BaseController
{
    private readonly ICapPublisher _capPublisher;

    public PublishController(ICapPublisher capPublisher)
    {
        _capPublisher = capPublisher;
    }

    [Route("~/send")]
    [HttpGet]
    public IActionResult SendMessage()
    {
        for (int i = 0; i < 100; i++)
        {
            _capPublisher.Publish("test.show.time", DateTime.Now);
        }

        return Ok();
    }

    [HttpGet]
    public IActionResult Test()
    {
        _capPublisher.PublishDelay(TimeSpan.FromSeconds(10), "test.show.time", DateTime.Now);

        return Ok();
    }
}