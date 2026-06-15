using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/second")]
public class SecondController : ControllerBase
{
    private readonly RequestCounterService _counter;

    public SecondController(RequestCounterService counter)
    {
        _counter = counter;
    }

    [HttpGet]
    public IActionResult Get()
    {
        _counter.Increment();
        return Ok(new
        {
            Controller = "Second",
            _counter.Id,
            _counter.Count
        });
    }
}