using Microsoft.AspNetCore.Mvc;

namespace Hebi_Api.Features.Users.Controllers;
public class UsersController : ControllerBase
{
    public IActionResult Index()
    {
        return Ok();
    }
}
