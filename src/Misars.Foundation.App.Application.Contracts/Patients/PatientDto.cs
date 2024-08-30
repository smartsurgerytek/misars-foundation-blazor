using System;
using System.Collections.Generic;

using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace Misars.Foundation.App.Patients
{
    public abstract class PatientDtoBase : FullAuditedEntityDto<Guid>, IHasConcurrencyStamp
    {
        public string? name { get; set; }
        public string? PatientID { get; set; }
        public DateTime DateofBirth { get; set; }
        public string? Gender { get; set; }
        public string? MedicalHistory { get; set; }

        public string ConcurrencyStamp { get; set; } = null!;

    }
}