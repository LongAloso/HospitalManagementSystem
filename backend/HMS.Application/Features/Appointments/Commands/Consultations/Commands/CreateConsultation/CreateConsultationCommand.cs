using MediatR;

namespace HMS.Application.Features.Consultations.Commands.CreateConsultation;

public record CreateConsultationRequest(
    string BloodPressure, decimal Temperature, decimal Weight, decimal Height, int HeartRate,
    string Symptoms, string Diagnosis, string Icd10Code, string Icd10Name
);

public record CreateConsultationCommand(
    Guid UserId,
    Guid AppointmentId,
    CreateConsultationRequest Data
) : IRequest<Guid>;