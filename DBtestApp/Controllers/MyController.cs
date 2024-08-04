using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class MyController : ControllerBase
{
    [HttpGet("specific-path")]
    public IActionResult Get()
    {
        return Ok("This is a specific path endpoint");
    }
}
