using Microsoft.AspNetCore.Mvc;
using privaxnet_api.ViewModels;
using privaxnet_api.Dtos;


namespace privaxnet_api.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    [HttpPost("create")]
    public ActionResult<UserViewModel> GetToken(UserDto sessionDto){

        var user = new UserViewModel();
        return Ok(user);

    }
}