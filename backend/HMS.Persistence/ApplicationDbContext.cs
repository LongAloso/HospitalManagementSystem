using HMS.Application.Interfaces;
using HMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HMS.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<MedicalHistory> MedicalHistories { get; set; }
    public DbSet<PatientAllergy> PatientAllergies { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();
        modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();

        // Cấu hình quan hệ 1-1: Patient - MedicalHistory
        modelBuilder.Entity<Patient>()
            .HasOne(p => p.MedicalHistory)
            .WithOne()
            .HasForeignKey<MedicalHistory>(m => m.PatientId)
            .OnDelete(DeleteBehavior.Cascade); // Xóa hồ sơ thì xóa luôn tiền sử bệnh

        // Cấu hình quan hệ 1-N: Patient - PatientAllergies
        modelBuilder.Entity<Patient>()
            .HasMany(p => p.Allergies)
            .WithOne()
            .HasForeignKey(a => a.PatientId)
            .OnDelete(DeleteBehavior.Cascade);

        // Đảm bảo mỗi CCCD là duy nhất
        modelBuilder.Entity<Patient>()
            .HasIndex(p => p.CitizenId)
            .IsUnique();
    }
}