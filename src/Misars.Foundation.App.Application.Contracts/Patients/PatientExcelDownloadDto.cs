using Volo.Abp.Application.Dtos;
using System;

namespace Misars.Foundation.App.Patients
{
    public abstract class PatientExcelDownloadDtoBase
    {
        public string DownloadToken { get; set; } = null!;

        public string? FilterText { get; set; }

        public string? name { get; set; }
        public string? PatientID { get; set; }
        public DateTime? DateofBirthMin { get; set; }
        public DateTime? DateofBirthMax { get; set; }
        public string? Gender { get; set; }
        public string? MedicalHistory { get; set; }

        public PatientExcelDownloadDtoBase()
        {

        }
    }
}