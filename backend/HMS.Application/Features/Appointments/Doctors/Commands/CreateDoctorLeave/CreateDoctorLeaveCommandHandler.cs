using HMS.Application.Interfaces;
using HMS.Domain.Entities;
using HMS.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HMS.Application.Features.Doctors.Commands.CreateDoctorLeave;

public class CreateDoctorLeaveCommandHandler : IRequestHandler<CreateDoctorLeaveCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly IEmailService _emailService;

    public CreateDoctorLeaveCommandHandler(IApplicationDbContext context, IEmailService emailService)
    {
        _context = context;
        _emailService = emailService;
    }

    public async Task<bool> Handle(CreateDoctorLeaveCommand request, CancellationToken cancellationToken)
    {
        
        var doctorLeave = new DoctorLeave
        {
            DoctorId = request.DoctorId,
            LeaveDate = request.LeaveDate.Date,
            Reason = request.Reason
        };
        _context.DoctorLeaves.Add(doctorLeave);

        
        var affectedAppointments = await _context.Appointments
            .Include(a => a.Patient) 
            .Where(a => a.DoctorId == request.DoctorId
                     && a.AppointmentDate.Date == request.LeaveDate.Date
                     && (a.Status == AppointmentStatus.Pending || a.Status == AppointmentStatus.Confirmed))
            .ToListAsync(cancellationToken);

        
        foreach (var appointment in affectedAppointments)
        {
            appointment.Status = AppointmentStatus.Cancelled;

           
            var patientName = appointment.Patient?.FullName ?? "Bệnh nhân";
            var emailBody = $"Chào {patientName}, lịch khám của bạn vào ngày {appointment.AppointmentDate:dd/MM/yyyy} lúc {appointment.StartTime} đã bị hủy do bác sĩ có việc đột xuất. Xin lỗi bạn vì sự bất tiện này.";

            await _emailService.SendEmailAsync("patient@example.com", "Thông báo hủy lịch hẹn khám", emailBody);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}