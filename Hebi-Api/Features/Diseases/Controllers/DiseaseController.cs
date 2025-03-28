using Microsoft.AspNetCore.Mvc;

namespace Hebi_Api.Features.Diseases.Controllers;
public class DiseaseController : ControllerBase
{
    public IActionResult Index()
    {
        return Ok();
    }
}
