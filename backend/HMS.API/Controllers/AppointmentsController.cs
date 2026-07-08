using HMS.API.Filters;
using HMS.Application.Features.Appointments.Commands.ApproveAppointment;
using HMS.Application.Features.Appointments.Commands.CreateAppointment;
using HMS.Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HMS.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AppointmentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AppointmentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> BookAppointment([FromBody] CreateAppointmentRequest request)
    {
        try
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out Guid tokenUserId))
            {
                return Unauthorized("Không xác định được danh tính.");
            }

            var command = new CreateAppointmentCommand(
                tokenUserId,
                request.DoctorId,
                request.AppointmentDate,
                request.StartTime,
                request.EndTime,
                request.Reason
            );

            var appointmentId = await _mediator.Send(command);
            return Ok(new { AppointmentId = appointmentId, Message = "Đặt lịch hẹn thành công! Vui lòng chờ xác nhận." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }
    [HttpPut("{id}/approve")]
    [HasPermission(Permissions.ApproveAppointment)] 
    public async Task<IActionResult> Approve(Guid id, [FromBody] byte[] rowVersion)
    {
        try
        {
            var command = new ApproveAppointmentCommand(id, rowVersion);
            await _mediator.Send(command);

            return Ok(new { Message = "Duyệt lịch hẹn thành công!" });
        }
        catch (DbUpdateConcurrencyException ex) 
        {
            return Conflict(new { Error = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }
}