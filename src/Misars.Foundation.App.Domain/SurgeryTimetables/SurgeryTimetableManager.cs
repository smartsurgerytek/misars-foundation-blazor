using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Data;

namespace Misars.Foundation.App.SurgeryTimetables
{
    public abstract class SurgeryTimetableManagerBase : DomainService
    {
        protected ISurgeryTimetableRepository _surgeryTimetableRepository;

        public SurgeryTimetableManagerBase(ISurgeryTimetableRepository surgeryTimetableRepository)
        {
            _surgeryTimetableRepository = surgeryTimetableRepository;
        }

        public virtual async Task<SurgeryTimetable> CreateAsync(
        Guid? doctorId, Guid? patientId, DateTime startdate, DateTime enddate, string? time = null, string? department = null, string? diagnosis = null, string? surgicalMethod = null, string? notes = null)
        {
            Check.NotNull(startdate, nameof(startdate));
            Check.NotNull(enddate, nameof(enddate));
            Check.Length(time, nameof(time), SurgeryTimetableConsts.TimeMaxLength, SurgeryTimetableConsts.TimeMinLength);
            Check.Length(diagnosis, nameof(diagnosis), SurgeryTimetableConsts.DiagnosisMaxLength, SurgeryTimetableConsts.DiagnosisMinLength);
            Check.Length(surgicalMethod, nameof(surgicalMethod), SurgeryTimetableConsts.SurgicalMethodMaxLength, SurgeryTimetableConsts.SurgicalMethodMinLength);
            Check.Length(notes, nameof(notes), SurgeryTimetableConsts.notesMaxLength, SurgeryTimetableConsts.notesMinLength);

            var surgeryTimetable = new SurgeryTimetable(
             GuidGenerator.Create(),
             doctorId, patientId, startdate, enddate, time, department, diagnosis, surgicalMethod, notes
             );

            return await _surgeryTimetableRepository.InsertAsync(surgeryTimetable);
        }

        public virtual async Task<SurgeryTimetable> UpdateAsync(
            Guid id,
            Guid? doctorId, Guid? patientId, DateTime startdate, DateTime enddate, string? time = null, string? department = null, string? diagnosis = null, string? surgicalMethod = null, string? notes = null, [CanBeNull] string? concurrencyStamp = null
        )
        {
            Check.NotNull(startdate, nameof(startdate));
            Check.NotNull(enddate, nameof(enddate));
            Check.Length(time, nameof(time), SurgeryTimetableConsts.TimeMaxLength, SurgeryTimetableConsts.TimeMinLength);
            Check.Length(diagnosis, nameof(diagnosis), SurgeryTimetableConsts.DiagnosisMaxLength, SurgeryTimetableConsts.DiagnosisMinLength);
            Check.Length(surgicalMethod, nameof(surgicalMethod), SurgeryTimetableConsts.SurgicalMethodMaxLength, SurgeryTimetableConsts.SurgicalMethodMinLength);
            Check.Length(notes, nameof(notes), SurgeryTimetableConsts.notesMaxLength, SurgeryTimetableConsts.notesMinLength);

            var surgeryTimetable = await _surgeryTimetableRepository.GetAsync(id);

            surgeryTimetable.DoctorId = doctorId;
            surgeryTimetable.PatientId = patientId;
            surgeryTimetable.startdate = startdate;
            surgeryTimetable.enddate = enddate;
            surgeryTimetable.Time = time;
            surgeryTimetable.Department = department;
            surgeryTimetable.Diagnosis = diagnosis;
            surgeryTimetable.SurgicalMethod = surgicalMethod;
            surgeryTimetable.notes = notes;

            surgeryTimetable.SetConcurrencyStampIfNotNull(concurrencyStamp);
            return await _surgeryTimetableRepository.UpdateAsync(surgeryTimetable);
        }

    }
}