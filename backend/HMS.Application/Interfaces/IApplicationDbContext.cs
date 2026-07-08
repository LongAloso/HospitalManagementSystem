using HMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace HMS.Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    DbSet<Patient> Patients { get; }
    DbSet<MedicalHistory> MedicalHistories { get; }
    DbSet<PatientAllergy> PatientAllergies { get; }
    DbSet<Doctor> Doctors { get; }
    DbSet<Appointment> Appointments { get; }
    DbSet<DoctorLeave> DoctorLeaves { get; }
    EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
    DbSet<Consultation> Consultations { get; }
    DbSet<Medicine> Medicines { get; }
    DbSet<Prescription> Prescriptions { get; }
    DbSet<PrescriptionItem> PrescriptionItems { get; }
}