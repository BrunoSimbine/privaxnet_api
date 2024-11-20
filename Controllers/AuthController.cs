using Microsoft.AspNetCore.Mvc;
using privaxnet_api.ViewModels;


namespace privaxnet_api.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    [HttpPost("login")]
    public ActionResult<SessionViewModel> GetToken(SessionDto sessionDto){

        var login = new SessionViewModel();
        return Ok(login);

    }

    [HttpGet("status")]
    public ActionResult GetToken([FromQuery string token])
    {
        var login = new SessionViewModel();
        return Ok(login);

    }  

}
