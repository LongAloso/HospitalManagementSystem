using HMS.Application.Interfaces;
using HMS.Domain.Entities;
using HMS.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HMS.Application.Features.Appointments.Commands.CreateAppointment;

public class CreateAppointmentCommandHandler : IRequestHandler<CreateAppointmentCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateAppointmentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateAppointmentCommand request, CancellationToken cancellationToken)
    {
        var patient = await _context.Patients
            .FirstOrDefaultAsync(p => p.UserId == request.UserId, cancellationToken);

        if (patient == null)
        {
            throw new Exception("Vui lòng tạo hồ sơ bệnh nhân trước khi đặt lịch.");
        }

        var existingAppointments = await _context.Appointments
            .Where(a => a.DoctorId == request.DoctorId
                     && a.AppointmentDate.Date == request.AppointmentDate.Date
                     && a.Status != AppointmentStatus.Cancelled
                     && a.Status != AppointmentStatus.Rejected)
            .ToListAsync(cancellationToken);

        bool isOverlapping = existingAppointments.Any(a =>
            (request.StartTime >= a.StartTime && request.StartTime < a.EndTime) ||
            (request.EndTime > a.StartTime && request.EndTime <= a.EndTime) ||
            (request.StartTime <= a.StartTime && request.EndTime >= a.EndTime));

        if (isOverlapping)
        {
            throw new Exception("Khung giờ này bác sĩ đã có lịch hẹn. Vui lòng chọn giờ khác.");
        }

        var appointment = new Appointment
        {
            PatientId = patient.Id,
            DoctorId = request.DoctorId,
            AppointmentDate = request.AppointmentDate.Date,
            StartTime = request.StartTime,
            EndTime = request.EndTime,
            Reason = request.Reason,
            Status = AppointmentStatus.Pending
        };

        _context.Appointments.Add(appointment);

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex)
        {
            if (ex.InnerException != null && ex.InnerException.Message.Contains("IX_Appointment_Doctor_Date_Time"))
            {
                throw new Exception("Rất tiếc, khung giờ này vừa có người đặt. Vui lòng chọn giờ khác.");
            }
            throw;
        }

        return appointment.Id;
    }
}