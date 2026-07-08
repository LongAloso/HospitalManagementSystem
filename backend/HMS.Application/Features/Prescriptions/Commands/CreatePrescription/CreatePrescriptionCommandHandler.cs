using HMS.Application.Interfaces;
using HMS.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HMS.Application.Features.Prescriptions.Commands.CreatePrescription;

public class CreatePrescriptionCommandHandler : IRequestHandler<CreatePrescriptionCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreatePrescriptionCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreatePrescriptionCommand request, CancellationToken cancellationToken)
    {
        var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.UserId == request.UserId, cancellationToken);
        if (doctor == null) throw new UnauthorizedAccessException("Bạn không phải là bác sĩ.");

        var consultation = await _context.Consultations
            .Include(c => c.Patient)
            .ThenInclude(p => p!.Allergies) 
            .FirstOrDefaultAsync(c => c.Id == request.Data.ConsultationId, cancellationToken);

        if (consultation == null) throw new KeyNotFoundException("Không tìm thấy Phiếu khám lâm sàng.");
        if (consultation.DoctorId != doctor.Id) throw new UnauthorizedAccessException("Bạn không thể kê đơn cho phiếu khám của người khác.");

        var medicineIds = request.Data.Items.Select(i => i.MedicineId).ToList();

        var medicinesInDb = await _context.Medicines
            .Where(m => medicineIds.Contains(m.Id))
            .ToListAsync(cancellationToken);

        var prescriptionItems = new List<PrescriptionItem>();


        foreach (var itemDto in request.Data.Items)
        {
            var medicine = medicinesInDb.FirstOrDefault(m => m.Id == itemDto.MedicineId);
            if (medicine == null) throw new Exception($"Thuốc với ID {itemDto.MedicineId} không tồn tại.");


            if (medicine.StockQuantity < itemDto.Quantity)
            {
                throw new InvalidOperationException($"Thuốc {medicine.Name} chỉ còn {medicine.StockQuantity} trong kho. Không đủ để kê {itemDto.Quantity}.");
            }


            bool isAllergic = consultation.Patient!.Allergies.Any(a =>
                medicine.ActiveIngredient.Contains(a.Allergen, StringComparison.OrdinalIgnoreCase) ||
                medicine.Name.Contains(a.Allergen, StringComparison.OrdinalIgnoreCase));

            if (isAllergic)
            {

                throw new InvalidOperationException($"CẢNH BÁO ĐỎ: Bệnh nhân có tiền sử dị ứng với thành phần của thuốc {medicine.Name} ({medicine.ActiveIngredient}). Vui lòng kê thuốc khác.");
            }

            prescriptionItems.Add(new PrescriptionItem
            {
                MedicineId = itemDto.MedicineId,
                Quantity = itemDto.Quantity,
                DurationDays = itemDto.DurationDays,
                Instruction = itemDto.Instruction
            });
        }

        var prescription = new Prescription
        {
            ConsultationId = consultation.Id,
            DoctorId = doctor.Id,
            PatientId = consultation.PatientId,
            Notes = request.Data.Notes,
            Items = prescriptionItems
        };

        _context.Prescriptions.Add(prescription);
        await _context.SaveChangesAsync(cancellationToken);

        return prescription.Id;
    }
}