using HMS.Application.Interfaces;
using HMS.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HMS.Application.Features.Patients.Commands.CreatePatient;

public class CreatePatientCommandHandler : IRequestHandler<CreatePatientCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreatePatientCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreatePatientCommand request, CancellationToken cancellationToken)
    {
        if (await _context.Patients.AnyAsync(p => p.UserId == request.UserId, cancellationToken))
        {
            throw new Exception("Tài khoản này đã có hồ sơ bệnh nhân.");
        }

        var patient = new Patient
        {
            UserId = request.UserId,
            FullName = request.FullName,
            DateOfBirth = request.DateOfBirth,
            Gender = request.Gender,
            PhoneNumber = request.PhoneNumber,
            CitizenId = request.CitizenId,
            Address = request.Address,
            BloodType = request.BloodType,
            EmergencyContact = request.EmergencyContact
        };

        var medicalHistory = new MedicalHistory
        {
            PatientId = patient.Id,
            HasDiabetes = request.HasDiabetes,
            HasHypertension = request.HasHypertension,
            HasAsthma = request.HasAsthma,
            HasCancerHistory = request.HasCancerHistory,
            PreviousSurgeries = request.PreviousSurgeries,
            FamilyDisease = request.FamilyDisease
        };

        var allergies = request.Allergies.Select(a => new PatientAllergy
        {
            PatientId = patient.Id,
            Allergen = a.Allergen,
            Severity = a.Severity
        }).ToList();

        _context.Patients.Add(patient);
        _context.MedicalHistories.Add(medicalHistory);
        _context.PatientAllergies.AddRange(allergies);

        await _context.SaveChangesAsync(cancellationToken);

        return patient.Id;
    }
}