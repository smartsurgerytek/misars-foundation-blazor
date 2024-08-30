using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Misars.Foundation.App.SurgeryTimetables
{
    public abstract class SurgeryTimetableCreateDtoBase
    {
        public DateTime startdate { get; set; }
        public DateTime enddate { get; set; }
        [StringLength(SurgeryTimetableConsts.TimeMaxLength, MinimumLength = SurgeryTimetableConsts.TimeMinLength)]
        public string? Time { get; set; }
        public string? Department { get; set; }
        [StringLength(SurgeryTimetableConsts.DiagnosisMaxLength, MinimumLength = SurgeryTimetableConsts.DiagnosisMinLength)]
        public string? Diagnosis { get; set; }
        [StringLength(SurgeryTimetableConsts.SurgicalMethodMaxLength, MinimumLength = SurgeryTimetableConsts.SurgicalMethodMinLength)]
        public string? SurgicalMethod { get; set; }
        [StringLength(SurgeryTimetableConsts.notesMaxLength, MinimumLength = SurgeryTimetableConsts.notesMinLength)]
        public string? notes { get; set; }
        public Guid? DoctorId { get; set; }
        public Guid? PatientId { get; set; }
    }
}