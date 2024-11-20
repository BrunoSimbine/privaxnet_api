using Microsoft.AspNetCore.Mvc;
using privaxnet_api.ViewModels;


namespace privaxnet_api.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    [HttpGet("login")]
    public ActionResult<LoginViewModel> GetToken(){

        var login = new LoginViewModel();
        return Ok(login);

    }


}
