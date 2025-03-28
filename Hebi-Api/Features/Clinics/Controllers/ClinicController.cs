using Microsoft.AspNetCore.Mvc;

namespace Hebi_Api.Features.Clinics.Controllers;
public class ClinicController : ControllerBase
{
    public IActionResult Index()
    {
        return Ok();
    }
}
