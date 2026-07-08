using MediatR;

namespace HMS.Application.Features.Appointments.Commands.CreateAppointment;
public record CreateAppointmentRequest(
    Guid DoctorId,
    DateTime AppointmentDate,
    TimeSpan StartTime,
    TimeSpan EndTime,
    string Reason
);

public record CreateAppointmentCommand(
    Guid UserId, 
    Guid DoctorId,
    DateTime AppointmentDate,
    TimeSpan StartTime,
    TimeSpan EndTime,
    string Reason
) : IRequest<Guid>;