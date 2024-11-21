using Microsoft.AspNetCore.Mvc;
using privaxnet_api.ViewModels;
using privaxnet_api.Dtos;
using privaxnet_api.Data;
using privaxnet_api.Models;


namespace privaxnet_api.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly DataContext _context;

    public AuthController(DataContext context)
    {
        _context = context;
    }

    [HttpPost("login")]
    public ActionResult<SessionViewModel> GetToken(SessionDto sessionDto)
    {

        var login = new SessionViewModel();
        return Ok(login);

    }

    [HttpGet("status")]
    public ActionResult GetToken([FromQuery] string token)
    {
        var login = new SessionViewModel();
        return Ok(login);

    }  

}
