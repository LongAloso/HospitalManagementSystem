using MediatR;

namespace HMS.Application.Features.Prescriptions.Commands.CreatePrescription;

public record PrescriptionItemDto(Guid MedicineId, int Quantity, int DurationDays, string Instruction);

public record CreatePrescriptionRequest(Guid ConsultationId, string Notes, List<PrescriptionItemDto> Items);

public record CreatePrescriptionCommand(Guid UserId, CreatePrescriptionRequest Data) : IRequest<Guid>;