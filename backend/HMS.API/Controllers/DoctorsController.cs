using HMS.Application.Features.Doctors.Commands.CreateDoctorLeave;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HMS.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize] // Bắt buộc đăng nhập
public class DoctorsController : ControllerBase
{
    private readonly IMediator _mediator;

    public DoctorsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("{doctorId}/leaves")]
    public async Task<IActionResult> ReportLeave(Guid doctorId, [FromBody] CreateDoctorLeaveRequest request)
    {
        try
        {
            var command = new CreateDoctorLeaveCommand(doctorId, request.LeaveDate, request.Reason);
            await _mediator.Send(command);

            return Ok(new { Message = "Đã ghi nhận ngày nghỉ. Các lịch hẹn liên quan đã được hủy và thông báo." });
        }
        catch (DbUpdateException)
        {
            
            return BadRequest(new { Error = "Bác sĩ này đã được báo nghỉ trong ngày này rồi." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Error = "Đã xảy ra lỗi hệ thống", Detail = ex.Message });
        }
    }
}

public class CreateDoctorLeaveRequest
{
    public DateTime LeaveDate { get; set; }
    public string Reason { get; set; } = string.Empty;
}