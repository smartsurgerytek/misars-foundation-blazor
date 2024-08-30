using System;

namespace Misars.Foundation.App.Patients
{
    public abstract class PatientExcelDtoBase
    {
        public string? name { get; set; }
        public string? PatientID { get; set; }
        public DateTime DateofBirth { get; set; }
        public string? Gender { get; set; }
        public string? MedicalHistory { get; set; }
    }
}