using HMS.Application.DTOs;
using MediatR;

namespace HMS.Application.Features.Patients.Commands.CreatePatient;

public record CreatePatientCommand(
    Guid UserId,
    string FullName,
    DateTime DateOfBirth,
    string Gender,
    string PhoneNumber,
    string CitizenId,
    string Address,
    string BloodType,
    string EmergencyContact,

    bool HasDiabetes,
    bool HasHypertension,
    bool HasAsthma,
    bool HasCancerHistory,
    string PreviousSurgeries,
    string FamilyDisease,

    List<AllergyDto> Allergies
) : IRequest<Guid>; 