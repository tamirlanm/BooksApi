
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/first")]
public class FirstController : ControllerBase
{
    private readonly RequestCounterService _counter;

    public FirstController(RequestCounterService counter)
    {
        _counter = counter;
    }

    [HttpGet]
    public IActionResult Get()
    {
        _counter.Increment();
        return Ok(new
        {
            Controller = "First",
            _counter.Id,
            _counter.Count
        });
    }
}