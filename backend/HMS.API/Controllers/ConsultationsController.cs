using HMS.Application.Features.Consultations.Commands.CreateConsultation;
using HMS.API.Filters;
using HMS.Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HMS.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ConsultationsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ConsultationsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("appointments/{appointmentId}")]
    [HasPermission(Permissions.CreatePrescription)]
    public async Task<IActionResult> CreateConsultation(Guid appointmentId, [FromBody] CreateConsultationRequest request)
    {
        try
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out Guid tokenUserId))
            {
                return Unauthorized("Không xác định được danh tính.");
            }

            var command = new CreateConsultationCommand(tokenUserId, appointmentId, request);
            var consultationId = await _mediator.Send(command);

            return Ok(new
            {
                ConsultationId = consultationId,
                Message = "Lưu kết quả khám thành công. Trạng thái lịch hẹn đã chuyển sang Hoàn thành."
            });
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(403, new { Error = ex.Message }); 
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }
}