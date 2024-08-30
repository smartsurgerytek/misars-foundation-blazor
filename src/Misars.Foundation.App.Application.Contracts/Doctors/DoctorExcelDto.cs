using System;

namespace Misars.Foundation.App.Doctors
{
    public abstract class DoctorExcelDtoBase
    {
        public string? name { get; set; }
        public string? DoctorID { get; set; }
        public string? Specialty { get; set; }
        public string? Department { get; set; }
    }
}