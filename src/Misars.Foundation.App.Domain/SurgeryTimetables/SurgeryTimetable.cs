using Misars.Foundation.App.Doctors;
using Misars.Foundation.App.Patients;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using JetBrains.Annotations;

using Volo.Abp;

namespace Misars.Foundation.App.SurgeryTimetables
{
    public abstract class SurgeryTimetableBase : FullAuditedAggregateRoot<Guid>
    {
        public virtual DateTime startdate { get; set; }

        public virtual DateTime enddate { get; set; }

        [CanBeNull]
        public virtual string? Time { get; set; }

        [CanBeNull]
        public virtual string? Department { get; set; }

        [CanBeNull]
        public virtual string? Diagnosis { get; set; }

        [CanBeNull]
        public virtual string? SurgicalMethod { get; set; }

        [CanBeNull]
        public virtual string? notes { get; set; }
        public Guid? DoctorId { get; set; }
        public Guid? PatientId { get; set; }

        protected SurgeryTimetableBase()
        {

        }

        public SurgeryTimetableBase(Guid id, Guid? doctorId, Guid? patientId, DateTime startdate, DateTime enddate, string? time = null, string? department = null, string? diagnosis = null, string? surgicalMethod = null, string? notes = null)
        {

            Id = id;
            Check.Length(time, nameof(time), SurgeryTimetableConsts.TimeMaxLength, SurgeryTimetableConsts.TimeMinLength);
            Check.Length(diagnosis, nameof(diagnosis), SurgeryTimetableConsts.DiagnosisMaxLength, SurgeryTimetableConsts.DiagnosisMinLength);
            Check.Length(surgicalMethod, nameof(surgicalMethod), SurgeryTimetableConsts.SurgicalMethodMaxLength, SurgeryTimetableConsts.SurgicalMethodMinLength);
            Check.Length(notes, nameof(notes), SurgeryTimetableConsts.notesMaxLength, SurgeryTimetableConsts.notesMinLength);
            this.startdate = startdate;
            this.enddate = enddate;
            Time = time;
            Department = department;
            Diagnosis = diagnosis;
            SurgicalMethod = surgicalMethod;
            this.notes = notes;
            DoctorId = doctorId;
            PatientId = patientId;
        }

    }
}