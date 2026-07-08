using HMS.Application.Interfaces;
using HMS.Domain.Entities;
using HMS.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HMS.Application.Features.Consultations.Commands.CreateConsultation;

public class CreateConsultationCommandHandler : IRequestHandler<CreateConsultationCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateConsultationCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateConsultationCommand request, CancellationToken cancellationToken)
    {
        var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.UserId == request.UserId, cancellationToken);
        if (doctor == null)
        {
            throw new UnauthorizedAccessException("Tài khoản của bạn không có hồ sơ bác sĩ.");
        }

        var appointment = await _context.Appointments
            .FirstOrDefaultAsync(a => a.Id == request.AppointmentId, cancellationToken);

        if (appointment == null) throw new KeyNotFoundException("Không tìm thấy lịch hẹn.");

        if (appointment.DoctorId != doctor.Id)
            throw new UnauthorizedAccessException("Bạn không được phép khám bệnh nhân của bác sĩ khác."); // Chống hack đổi ID

        if (appointment.Status != AppointmentStatus.Confirmed && appointment.Status != AppointmentStatus.InProgress)
            throw new InvalidOperationException("Chỉ có thể khám cho lịch hẹn đã xác nhận hoặc đang tiến hành.");

        var consultation = new Consultation
        {
            AppointmentId = appointment.Id,
            PatientId = appointment.PatientId,
            DoctorId = doctor.Id,

            BloodPressure = request.Data.BloodPressure,
            Temperature = request.Data.Temperature,
            Weight = request.Data.Weight,
            Height = request.Data.Height,
            HeartRate = request.Data.HeartRate,

            Symptoms = request.Data.Symptoms,
            Diagnosis = request.Data.Diagnosis,
            Icd10Code = request.Data.Icd10Code,
            Icd10Name = request.Data.Icd10Name
        };
      
        appointment.Status = AppointmentStatus.Completed;

        _context.Consultations.Add(consultation);
        await _context.SaveChangesAsync(cancellationToken);

        return consultation.Id;
    }
}