using Microsoft.AspNetCore.Mvc;

namespace Hebi_Api.Features.Shifts.Controllers;
public class ShiftsController : ControllerBase
{
    public IActionResult Index()
    {
        return Ok();
    }
}
