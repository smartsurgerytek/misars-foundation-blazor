using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using JetBrains.Annotations;

using Volo.Abp;

namespace Misars.Foundation.App.Doctors
{
    public abstract class DoctorBase : FullAuditedAggregateRoot<Guid>
    {
        [CanBeNull]
        public virtual string? name { get; set; }

        [CanBeNull]
        public virtual string? DoctorID { get; set; }

        [CanBeNull]
        public virtual string? Specialty { get; set; }

        [CanBeNull]
        public virtual string? Department { get; set; }

        protected DoctorBase()
        {

        }

        public DoctorBase(Guid id, string? name = null, string? doctorID = null, string? specialty = null, string? department = null)
        {

            Id = id;
            Check.Length(name, nameof(name), DoctorConsts.nameMaxLength, DoctorConsts.nameMinLength);
            Check.Length(doctorID, nameof(doctorID), DoctorConsts.DoctorIDMaxLength, 0);
            Check.Length(specialty, nameof(specialty), DoctorConsts.SpecialtyMaxLength, 0);
            Check.Length(department, nameof(department), DoctorConsts.DepartmentMaxLength, DoctorConsts.DepartmentMinLength);
            this.name = name;
            DoctorID = doctorID;
            Specialty = specialty;
            Department = department;
        }

    }
}