using Microsoft.AspNetCore.Mvc;

namespace Hebi_Api.Features.UserCards.Controllers;
public class UserCardsController : ControllerBase
{
    public IActionResult Index()
    {
        return Ok();
    }
}
