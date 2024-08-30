using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using JetBrains.Annotations;

using Volo.Abp;

namespace Misars.Foundation.App.Patients
{
    public abstract class PatientBase : FullAuditedAggregateRoot<Guid>
    {
        [CanBeNull]
        public virtual string? name { get; set; }

        [CanBeNull]
        public virtual string? PatientID { get; set; }

        public virtual DateTime DateofBirth { get; set; }

        [CanBeNull]
        public virtual string? Gender { get; set; }

        [CanBeNull]
        public virtual string? MedicalHistory { get; set; }

        protected PatientBase()
        {

        }

        public PatientBase(Guid id, DateTime dateofBirth, string? name = null, string? patientID = null, string? gender = null, string? medicalHistory = null)
        {

            Id = id;
            Check.Length(name, nameof(name), PatientConsts.nameMaxLength, PatientConsts.nameMinLength);
            Check.Length(patientID, nameof(patientID), PatientConsts.PatientIDMaxLength, PatientConsts.PatientIDMinLength);
            Check.Length(gender, nameof(gender), PatientConsts.GenderMaxLength, PatientConsts.GenderMinLength);
            Check.Length(medicalHistory, nameof(medicalHistory), PatientConsts.MedicalHistoryMaxLength, PatientConsts.MedicalHistoryMinLength);
            DateofBirth = dateofBirth;
            this.name = name;
            PatientID = patientID;
            Gender = gender;
            MedicalHistory = medicalHistory;
        }

    }
}