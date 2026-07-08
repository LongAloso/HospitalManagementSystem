using HMS.Application.Features.Prescriptions.Commands.CreatePrescription;
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
public class PrescriptionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PrescriptionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [HasPermission(Permissions.CreatePrescription)]
    public async Task<IActionResult> CreatePrescription([FromBody] CreatePrescriptionRequest request)
    {
        try
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out Guid tokenUserId))
            {
                return Unauthorized("Không xác định được danh tính.");
            }

            var command = new CreatePrescriptionCommand(tokenUserId, request);
            var prescriptionId = await _mediator.Send(command);

            return Ok(new
            {
                PrescriptionId = prescriptionId,
                Message = "Kê đơn thuốc thành công. Đã vượt qua kiểm tra an toàn dị ứng."
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { Error = "Cảnh báo Y khoa", Detail = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Error = ex.Message });
        }
    }
}