using MediatR;

namespace HMS.Application.Features.Appointments.Commands.ApproveAppointment;

public record ApproveAppointmentCommand(Guid AppointmentId, byte[] RowVersion) : IRequest<bool>;