using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Misars.Foundation.App.Doctors
{
    public abstract class DoctorCreateDtoBase
    {
        [StringLength(DoctorConsts.nameMaxLength, MinimumLength = DoctorConsts.nameMinLength)]
        public string? name { get; set; }
        [StringLength(DoctorConsts.DoctorIDMaxLength)]
        public string? DoctorID { get; set; }
        [StringLength(DoctorConsts.SpecialtyMaxLength)]
        public string? Specialty { get; set; }
        [StringLength(DoctorConsts.DepartmentMaxLength, MinimumLength = DoctorConsts.DepartmentMinLength)]
        public string? Department { get; set; }
    }
}