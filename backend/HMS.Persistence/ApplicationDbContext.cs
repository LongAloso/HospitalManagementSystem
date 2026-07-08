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
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<DoctorLeave> DoctorLeaves { get; set; }
    public DbSet<Consultation> Consultations { get; set; }
    public DbSet<Medicine> Medicines { get; set; }
    public DbSet<Prescription> Prescriptions { get; set; }
    public DbSet<PrescriptionItem> PrescriptionItems { get; set; }

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
            .OnDelete(DeleteBehavior.Cascade);

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

        // Cấu hình bảng Doctor
        modelBuilder.Entity<Doctor>()
            .HasOne<User>() 
            .WithOne()      
            .HasForeignKey<Doctor>(d => d.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Cấu hình Optimistic cho quá trình update
        modelBuilder.Entity<Appointment>()
            .Property(a => a.RowVersion)
            .IsRowVersion();

        // Một bác sĩ không thể đặt 2 lịch hẹt vào cùng ngày và cùng giờ bắt đầu
        modelBuilder.Entity<Appointment>()
            .HasIndex(a => new { a.DoctorId, a.AppointmentDate, a.StartTime })
            .IsUnique()
            .HasDatabaseName("IX_Appointment_Doctor_Date_Time");

        // Cấu hình khóa ngoại cho Appointment
        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.Patient)
            .WithMany()
            .HasForeignKey(a => a.PatientId)
            .OnDelete(DeleteBehavior.Restrict);

        // Cấu hình Appointment: Khóa ngoại trỏ về Doctor
        // DÙNG RESTRICT: Không cho phép xóa Bác sĩ nếu họ đang có Lịch hẹn
        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.Doctor)
            .WithMany(d => d.Appointments)
            .HasForeignKey(a => a.DoctorId)
            .OnDelete(DeleteBehavior.Restrict);

        // Cấu hình bảng DoctorLeave
        modelBuilder.Entity<DoctorLeave>()
            .HasOne(dl => dl.Doctor)
            .WithMany(d => d.Leaves)
            .HasForeignKey(dl => dl.DoctorId)
            .OnDelete(DeleteBehavior.Cascade);

        // Đảm bảo 1 bác sĩ không xin nghỉ 2 lần trong cùng 1 ngày
        modelBuilder.Entity<DoctorLeave>()
            .HasIndex(dl => new { dl.DoctorId, dl.LeaveDate })
            .IsUnique();
        // Cấu hình quan hệ 1-1 giữa Appointment và Consultation
        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.Consultation)
            .WithOne(c => c.Appointment)
            .HasForeignKey<Consultation>(c => c.AppointmentId)
            .OnDelete(DeleteBehavior.Restrict);

        // Các quan hệ khác (N-1)
        modelBuilder.Entity<Consultation>()
            .HasOne(c => c.Patient)
            .WithMany()
            .HasForeignKey(c => c.PatientId)
            .OnDelete(DeleteBehavior.NoAction); 

        modelBuilder.Entity<Consultation>()
            .HasOne(c => c.Doctor)
            .WithMany()
            .HasForeignKey(c => c.DoctorId)
            .OnDelete(DeleteBehavior.NoAction);

        // Quan hệ Consultation - Prescription (1-1)
        modelBuilder.Entity<Consultation>()
            .HasOne(c => c.Prescription)
            .WithOne(p => p.Consultation)
            .HasForeignKey<Prescription>(p => p.ConsultationId)
            .OnDelete(DeleteBehavior.Restrict);

        // Quan hệ Prescription - PrescriptionItem (1-N)
        modelBuilder.Entity<Prescription>()
            .HasMany(p => p.Items)
            .WithOne(pi => pi.Prescription)
            .HasForeignKey(pi => pi.PrescriptionId)
            .OnDelete(DeleteBehavior.Cascade);

        // Chống xóa thuốc nếu đã từng được kê đơn
        modelBuilder.Entity<PrescriptionItem>()
            .HasOne(pi => pi.Medicine)
            .WithMany()
            .HasForeignKey(pi => pi.MedicineId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}