﻿using Hebi_Api.Features.Appointments.RequestHandling.Requests;
using Hebi_Api.Features.Appointments.Services;
using Hebi_Api.Features.Core.Common.RequestHandling;
using MediatR;

namespace Hebi_Api.Features.Appointments.RequestHandling.Handlers;

public class CreateAppointmentRequestHandler : IRequestHandler<CreateAppointmentRequest, Response>
{
    private readonly IAppointmentsService _appoitmentsService;
    private readonly ILogger<CreateAppointmentRequestHandler> _logger;

    public CreateAppointmentRequestHandler(IAppointmentsService appoitmentsService, ILogger<CreateAppointmentRequestHandler> logger)
    {
        _appoitmentsService = appoitmentsService;
        _logger = logger;
    }

    public async Task<Response> Handle(CreateAppointmentRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _appoitmentsService.CreateAppointmentAsync(request.Dto);
            return Response.Ok(request.Id,result);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return Response.InternalServerError(request.Id, e);
        }
    }
}
