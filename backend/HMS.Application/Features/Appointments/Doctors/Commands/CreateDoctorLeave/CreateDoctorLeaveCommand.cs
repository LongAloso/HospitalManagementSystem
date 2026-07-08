using MediatR;

namespace HMS.Application.Features.Doctors.Commands.CreateDoctorLeave;

public record CreateDoctorLeaveCommand(Guid DoctorId, DateTime LeaveDate, string Reason) : IRequest<bool>;