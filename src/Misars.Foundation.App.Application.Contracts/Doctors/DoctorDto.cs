using System;
using System.Collections.Generic;

using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace Misars.Foundation.App.Doctors
{
    public abstract class DoctorDtoBase : FullAuditedEntityDto<Guid>, IHasConcurrencyStamp
    {
        public string? name { get; set; }
        public string? DoctorID { get; set; }
        public string? Specialty { get; set; }
        public string? Department { get; set; }

        public string ConcurrencyStamp { get; set; } = null!;

    }
}