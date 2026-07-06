using HMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HMS.Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    DbSet<Patient> Patients { get; }
    DbSet<MedicalHistory> MedicalHistories { get; }
    DbSet<PatientAllergy> PatientAllergies { get; }
}