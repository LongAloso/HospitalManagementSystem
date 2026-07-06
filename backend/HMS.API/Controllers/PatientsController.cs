using HMS.Application.Features.Patients.Commands.CreatePatient;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HMS.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize] 
public class PatientsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PatientsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateProfile([FromBody] CreatePatientCommand command)
    {
        try
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out Guid tokenUserId))
            {
                return Unauthorized("Không xác định được danh tính từ Token.");
            }

            var secureCommand = command with { UserId = tokenUserId };

            var patientId = await _mediator.Send(secureCommand);
            return Ok(new { PatientId = patientId, Message = "Tạo hồ sơ bệnh nhân thành công!" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }
}