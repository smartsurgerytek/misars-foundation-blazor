using Volo.Abp.Application.Dtos;
using System;

namespace Misars.Foundation.App.Doctors
{
    public abstract class DoctorExcelDownloadDtoBase
    {
        public string DownloadToken { get; set; } = null!;

        public string? FilterText { get; set; }

        public string? name { get; set; }
        public string? DoctorID { get; set; }
        public string? Specialty { get; set; }
        public string? Department { get; set; }

        public DoctorExcelDownloadDtoBase()
        {

        }
    }
}