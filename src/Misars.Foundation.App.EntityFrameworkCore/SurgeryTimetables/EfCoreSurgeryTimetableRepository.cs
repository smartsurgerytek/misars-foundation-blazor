using Misars.Foundation.App.Patients;
using Misars.Foundation.App.Doctors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Misars.Foundation.App.EntityFrameworkCore;

namespace Misars.Foundation.App.SurgeryTimetables
{
    public abstract class EfCoreSurgeryTimetableRepositoryBase : EfCoreRepository<AppDbContext, SurgeryTimetable, Guid>
    {
        public EfCoreSurgeryTimetableRepositoryBase(IDbContextProvider<AppDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public virtual async Task DeleteAllAsync(
            string? filterText = null,
                        DateTime? startdateMin = null,
            DateTime? startdateMax = null,
            DateTime? enddateMin = null,
            DateTime? enddateMax = null,
            string? time = null,
            string? department = null,
            string? diagnosis = null,
            string? surgicalMethod = null,
            string? notes = null,
            Guid? doctorId = null,
            Guid? patientId = null,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryForNavigationPropertiesAsync();

            query = ApplyFilter(query, filterText, startdateMin, startdateMax, enddateMin, enddateMax, time, department, diagnosis, surgicalMethod, notes, doctorId, patientId);

            var ids = query.Select(x => x.SurgeryTimetable.Id);
            await DeleteManyAsync(ids, cancellationToken: GetCancellationToken(cancellationToken));
        }

        public virtual async Task<SurgeryTimetableWithNavigationProperties> GetWithNavigationPropertiesAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();

            return (await GetDbSetAsync()).Where(b => b.Id == id)
                .Select(surgeryTimetable => new SurgeryTimetableWithNavigationProperties
                {
                    SurgeryTimetable = surgeryTimetable,
                    Doctor = dbContext.Set<Doctor>().FirstOrDefault(c => c.Id == surgeryTimetable.DoctorId),
                    Patient = dbContext.Set<Patient>().FirstOrDefault(c => c.Id == surgeryTimetable.PatientId)
                }).FirstOrDefault();
        }

        public virtual async Task<List<SurgeryTimetableWithNavigationProperties>> GetListWithNavigationPropertiesAsync(
            string? filterText = null,
            DateTime? startdateMin = null,
            DateTime? startdateMax = null,
            DateTime? enddateMin = null,
            DateTime? enddateMax = null,
            string? time = null,
            string? department = null,
            string? diagnosis = null,
            string? surgicalMethod = null,
            string? notes = null,
            Guid? doctorId = null,
            Guid? patientId = null,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryForNavigationPropertiesAsync();
            query = ApplyFilter(query, filterText, startdateMin, startdateMax, enddateMin, enddateMax, time, department, diagnosis, surgicalMethod, notes, doctorId, patientId);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? SurgeryTimetableConsts.GetDefaultSorting(true) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        protected virtual async Task<IQueryable<SurgeryTimetableWithNavigationProperties>> GetQueryForNavigationPropertiesAsync()
        {
            return from surgeryTimetable in (await GetDbSetAsync())
                   join doctor in (await GetDbContextAsync()).Set<Doctor>() on surgeryTimetable.DoctorId equals doctor.Id into doctors
                   from doctor in doctors.DefaultIfEmpty()
                   join patient in (await GetDbContextAsync()).Set<Patient>() on surgeryTimetable.PatientId equals patient.Id into patients
                   from patient in patients.DefaultIfEmpty()
                   select new SurgeryTimetableWithNavigationProperties
                   {
                       SurgeryTimetable = surgeryTimetable,
                       Doctor = doctor,
                       Patient = patient
                   };
        }

        protected virtual IQueryable<SurgeryTimetableWithNavigationProperties> ApplyFilter(
            IQueryable<SurgeryTimetableWithNavigationProperties> query,
            string? filterText,
            DateTime? startdateMin = null,
            DateTime? startdateMax = null,
            DateTime? enddateMin = null,
            DateTime? enddateMax = null,
            string? time = null,
            string? department = null,
            string? diagnosis = null,
            string? surgicalMethod = null,
            string? notes = null,
            Guid? doctorId = null,
            Guid? patientId = null)
        {
            return query
                .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.SurgeryTimetable.Time!.Contains(filterText!) || e.SurgeryTimetable.Department!.Contains(filterText!) || e.SurgeryTimetable.Diagnosis!.Contains(filterText!) || e.SurgeryTimetable.SurgicalMethod!.Contains(filterText!) || e.SurgeryTimetable.notes!.Contains(filterText!))
                    .WhereIf(startdateMin.HasValue, e => e.SurgeryTimetable.startdate >= startdateMin!.Value)
                    .WhereIf(startdateMax.HasValue, e => e.SurgeryTimetable.startdate <= startdateMax!.Value)
                    .WhereIf(enddateMin.HasValue, e => e.SurgeryTimetable.enddate >= enddateMin!.Value)
                    .WhereIf(enddateMax.HasValue, e => e.SurgeryTimetable.enddate <= enddateMax!.Value)
                    .WhereIf(!string.IsNullOrWhiteSpace(time), e => e.SurgeryTimetable.Time.Contains(time))
                    .WhereIf(!string.IsNullOrWhiteSpace(department), e => e.SurgeryTimetable.Department.Contains(department))
                    .WhereIf(!string.IsNullOrWhiteSpace(diagnosis), e => e.SurgeryTimetable.Diagnosis.Contains(diagnosis))
                    .WhereIf(!string.IsNullOrWhiteSpace(surgicalMethod), e => e.SurgeryTimetable.SurgicalMethod.Contains(surgicalMethod))
                    .WhereIf(!string.IsNullOrWhiteSpace(notes), e => e.SurgeryTimetable.notes.Contains(notes))
                    .WhereIf(doctorId != null && doctorId != Guid.Empty, e => e.Doctor != null && e.Doctor.Id == doctorId)
                    .WhereIf(patientId != null && patientId != Guid.Empty, e => e.Patient != null && e.Patient.Id == patientId);
        }

        public virtual async Task<List<SurgeryTimetable>> GetListAsync(
            string? filterText = null,
            DateTime? startdateMin = null,
            DateTime? startdateMax = null,
            DateTime? enddateMin = null,
            DateTime? enddateMax = null,
            string? time = null,
            string? department = null,
            string? diagnosis = null,
            string? surgicalMethod = null,
            string? notes = null,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetQueryableAsync()), filterText, startdateMin, startdateMax, enddateMin, enddateMax, time, department, diagnosis, surgicalMethod, notes);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? SurgeryTimetableConsts.GetDefaultSorting(false) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        public virtual async Task<long> GetCountAsync(
            string? filterText = null,
            DateTime? startdateMin = null,
            DateTime? startdateMax = null,
            DateTime? enddateMin = null,
            DateTime? enddateMax = null,
            string? time = null,
            string? department = null,
            string? diagnosis = null,
            string? surgicalMethod = null,
            string? notes = null,
            Guid? doctorId = null,
            Guid? patientId = null,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryForNavigationPropertiesAsync();
            query = ApplyFilter(query, filterText, startdateMin, startdateMax, enddateMin, enddateMax, time, department, diagnosis, surgicalMethod, notes, doctorId, patientId);
            return await query.LongCountAsync(GetCancellationToken(cancellationToken));
        }

        protected virtual IQueryable<SurgeryTimetable> ApplyFilter(
            IQueryable<SurgeryTimetable> query,
            string? filterText = null,
            DateTime? startdateMin = null,
            DateTime? startdateMax = null,
            DateTime? enddateMin = null,
            DateTime? enddateMax = null,
            string? time = null,
            string? department = null,
            string? diagnosis = null,
            string? surgicalMethod = null,
            string? notes = null)
        {
            return query
                    .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.Time!.Contains(filterText!) || e.Department!.Contains(filterText!) || e.Diagnosis!.Contains(filterText!) || e.SurgicalMethod!.Contains(filterText!) || e.notes!.Contains(filterText!))
                    .WhereIf(startdateMin.HasValue, e => e.startdate >= startdateMin!.Value)
                    .WhereIf(startdateMax.HasValue, e => e.startdate <= startdateMax!.Value)
                    .WhereIf(enddateMin.HasValue, e => e.enddate >= enddateMin!.Value)
                    .WhereIf(enddateMax.HasValue, e => e.enddate <= enddateMax!.Value)
                    .WhereIf(!string.IsNullOrWhiteSpace(time), e => e.Time.Contains(time))
                    .WhereIf(!string.IsNullOrWhiteSpace(department), e => e.Department.Contains(department))
                    .WhereIf(!string.IsNullOrWhiteSpace(diagnosis), e => e.Diagnosis.Contains(diagnosis))
                    .WhereIf(!string.IsNullOrWhiteSpace(surgicalMethod), e => e.SurgicalMethod.Contains(surgicalMethod))
                    .WhereIf(!string.IsNullOrWhiteSpace(notes), e => e.notes.Contains(notes));
        }
    }
}