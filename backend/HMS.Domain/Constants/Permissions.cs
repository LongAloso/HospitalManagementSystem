namespace HMS.Domain.Constants;

public static class Permissions
{
    // Bệnh nhân
    public const string ViewMedicalRecord = "CanViewMedicalRecord";

    // Bác sĩ
    public const string CreatePrescription = "CanCreatePrescription";
    public const string UpdatePrescription = "CanUpdatePrescription";
    public const string DeletePrescription = "CanDeletePrescription";

    // Lễ tân
    public const string ApproveAppointment = "CanApproveAppointment";
    public const string CancelAppointment = "CanCancelAppointment";
}