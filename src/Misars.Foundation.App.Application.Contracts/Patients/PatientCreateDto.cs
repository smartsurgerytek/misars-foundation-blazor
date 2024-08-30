using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Misars.Foundation.App.Patients
{
    public abstract class PatientCreateDtoBase
    {
        [StringLength(PatientConsts.nameMaxLength, MinimumLength = PatientConsts.nameMinLength)]
        public string? name { get; set; }
        [StringLength(PatientConsts.PatientIDMaxLength, MinimumLength = PatientConsts.PatientIDMinLength)]
        public string? PatientID { get; set; }
        public DateTime DateofBirth { get; set; }
        [StringLength(PatientConsts.GenderMaxLength, MinimumLength = PatientConsts.GenderMinLength)]
        public string? Gender { get; set; }
        [StringLength(PatientConsts.MedicalHistoryMaxLength, MinimumLength = PatientConsts.MedicalHistoryMinLength)]
        public string? MedicalHistory { get; set; }
    }
}