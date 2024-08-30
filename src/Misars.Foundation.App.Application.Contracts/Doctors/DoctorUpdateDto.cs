using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace Misars.Foundation.App.Doctors
{
    public abstract class DoctorUpdateDtoBase : IHasConcurrencyStamp
    {
        [StringLength(DoctorConsts.nameMaxLength, MinimumLength = DoctorConsts.nameMinLength)]
        public string? name { get; set; }
        [StringLength(DoctorConsts.DoctorIDMaxLength)]
        public string? DoctorID { get; set; }
        [StringLength(DoctorConsts.SpecialtyMaxLength)]
        public string? Specialty { get; set; }
        [StringLength(DoctorConsts.DepartmentMaxLength, MinimumLength = DoctorConsts.DepartmentMinLength)]
        public string? Department { get; set; }

        public string ConcurrencyStamp { get; set; } = null!;
    }
}