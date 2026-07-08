using HMS.Application.Interfaces;
using HMS.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HMS.Application.Features.Appointments.Commands.ApproveAppointment;

public class ApproveAppointmentCommandHandler : IRequestHandler<ApproveAppointmentCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public ApproveAppointmentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(ApproveAppointmentCommand request, CancellationToken cancellationToken)
    {
        var appointment = await _context.Appointments
            .FirstOrDefaultAsync(a => a.Id == request.AppointmentId, cancellationToken);

        if (appointment == null)
        {
            throw new KeyNotFoundException("Không tìm thấy lịch hẹn.");
        }

        if (appointment.Status != AppointmentStatus.Pending)
        {
            throw new InvalidOperationException("Chỉ có thể duyệt lịch hẹn đang ở trạng thái Chờ (Pending).");
        }

        _context.Entry(appointment).Property(a => a.RowVersion).OriginalValue = request.RowVersion;

        appointment.Status = AppointmentStatus.Confirmed;

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
        catch (DbUpdateConcurrencyException)
        {
            
            throw new Exception("Lịch hẹn này vừa được cập nhật bởi một người khác. Vui lòng tải lại trang.");
        }
    }
}