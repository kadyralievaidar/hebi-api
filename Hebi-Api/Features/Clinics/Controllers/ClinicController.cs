using Hebi_Api.Features.Clinics.Dtos;
using Hebi_Api.Features.Clinics.RequestHandling.Requests;
using Hebi_Api.Features.Core.Common;
using Hebi_Api.Features.Core.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;

namespace Hebi_Api.Features.Clinics.Controllers;

/// <summary>
///     Clinic's controller
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
public class ClinicController : ControllerBase
{
    private readonly IMediator _mediator;

    public ClinicController(IMediator mediator) => _mediator = mediator;

    /// <summary>
    ///     Create a clinic
    /// </summary>
    /// <param name="dto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    
    [Authorize(Roles = $"{Consts.Admin},{Consts.SuperAdmin}")]
    [HttpPost("create-clinic")]
    public async Task<IActionResult> Create([FromBody] CreateClinicDto dto, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CreateClinicRequest(dto), cancellationToken);
        return result.AsAspNetCoreResult();
    }

    /// <summary>
    ///     Update the clinic 
    /// </summary>
    /// <param name="clinicId"></param>
    /// <param name="dto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>

    [HttpPut("update")]
    public async Task<IActionResult> Update([FromQuery] Guid clinicId, [FromBody] CreateClinicDto dto, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new UpdateClinicRequest(clinicId, dto), cancellationToken);
        return result.AsAspNetCoreResult();
    }

    /// <summary>
    ///     Delete clinic by id
    /// </summary>
    /// <param name="clinicId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>

    [HttpDelete("id")]
    public async Task<IActionResult> Delete(Guid clinicId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new DeleteClinicRequest(clinicId),cancellationToken);
        return result.AsAspNetCoreResult();
    }
    
    /// <summary>
    ///     Get a clinic by id
    /// </summary>
    /// <param name="clinicId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>

    [HttpGet("id")]
    public async Task<IActionResult> GetById(Guid clinicId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetClinicByIdRequest(clinicId), cancellationToken);
        return result.AsAspNetCoreResult();
    }

    /// <summary>
    ///     Get a list of clinics
    /// </summary>
    /// <param name="dto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    
    [HttpGet("get")]
    public async Task<IActionResult> GetClinics([FromQuery] GetPagedListOfClinicDto dto, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetPagedListClinicRequest(dto), cancellationToken);
        return result.AsAspNetCoreResult();
    }

    /// <summary>
    ///     Get a clinic with doctors
    ///     Pagination applies to doctors
    /// </summary>
    /// <param name="dto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>

    [HttpGet("doctors")]
    public async Task<IActionResult> GetClinicWithDoctors([FromQuery]GetClinicsDoctorsDto dto, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetClinicWithDoctorsRequest(dto), cancellationToken);
        return result.AsAspNetCoreResult();
    }

    /// <summary>
    ///     Remove doctors from clinic
    /// </summary>
    /// <returns></returns>
    [HttpPut("remove-doctors")]
    public async Task<IActionResult> RemoveDoctorFromClinic([FromQuery]List<Guid> doctorIds)
    {
        var result = await _mediator.Send(new RemoveDoctorsRequest(doctorIds));
        return result.AsAspNetCoreResult();
    }
}
