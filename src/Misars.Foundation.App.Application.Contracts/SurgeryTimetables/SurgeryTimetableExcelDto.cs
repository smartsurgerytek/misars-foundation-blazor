using System;

namespace Misars.Foundation.App.SurgeryTimetables
{
    public abstract class SurgeryTimetableExcelDtoBase
    {
        public DateTime startdate { get; set; }
        public DateTime enddate { get; set; }
        public string? Time { get; set; }
        public string? Department { get; set; }
        public string? Diagnosis { get; set; }
        public string? SurgicalMethod { get; set; }
        public string? notes { get; set; }
    }
}