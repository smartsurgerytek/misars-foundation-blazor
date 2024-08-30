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

namespace Misars.Foundation.App.Patients
{
    public abstract class EfCorePatientRepositoryBase : EfCoreRepository<AppDbContext, Patient, Guid>
    {
        public EfCorePatientRepositoryBase(IDbContextProvider<AppDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public virtual async Task DeleteAllAsync(
            string? filterText = null,
                        string? name = null,
            string? patientID = null,
            DateTime? dateofBirthMin = null,
            DateTime? dateofBirthMax = null,
            string? gender = null,
            string? medicalHistory = null,
            CancellationToken cancellationToken = default)
        {

            var query = await GetQueryableAsync();

            query = ApplyFilter(query, filterText, name, patientID, dateofBirthMin, dateofBirthMax, gender, medicalHistory);

            var ids = query.Select(x => x.Id);
            await DeleteManyAsync(ids, cancellationToken: GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<Patient>> GetListAsync(
            string? filterText = null,
            string? name = null,
            string? patientID = null,
            DateTime? dateofBirthMin = null,
            DateTime? dateofBirthMax = null,
            string? gender = null,
            string? medicalHistory = null,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetQueryableAsync()), filterText, name, patientID, dateofBirthMin, dateofBirthMax, gender, medicalHistory);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? PatientConsts.GetDefaultSorting(false) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        public virtual async Task<long> GetCountAsync(
            string? filterText = null,
            string? name = null,
            string? patientID = null,
            DateTime? dateofBirthMin = null,
            DateTime? dateofBirthMax = null,
            string? gender = null,
            string? medicalHistory = null,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetDbSetAsync()), filterText, name, patientID, dateofBirthMin, dateofBirthMax, gender, medicalHistory);
            return await query.LongCountAsync(GetCancellationToken(cancellationToken));
        }

        protected virtual IQueryable<Patient> ApplyFilter(
            IQueryable<Patient> query,
            string? filterText = null,
            string? name = null,
            string? patientID = null,
            DateTime? dateofBirthMin = null,
            DateTime? dateofBirthMax = null,
            string? gender = null,
            string? medicalHistory = null)
        {
            return query
                    .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.name!.Contains(filterText!) || e.PatientID!.Contains(filterText!) || e.Gender!.Contains(filterText!) || e.MedicalHistory!.Contains(filterText!))
                    .WhereIf(!string.IsNullOrWhiteSpace(name), e => e.name.Contains(name))
                    .WhereIf(!string.IsNullOrWhiteSpace(patientID), e => e.PatientID.Contains(patientID))
                    .WhereIf(dateofBirthMin.HasValue, e => e.DateofBirth >= dateofBirthMin!.Value)
                    .WhereIf(dateofBirthMax.HasValue, e => e.DateofBirth <= dateofBirthMax!.Value)
                    .WhereIf(!string.IsNullOrWhiteSpace(gender), e => e.Gender.Contains(gender))
                    .WhereIf(!string.IsNullOrWhiteSpace(medicalHistory), e => e.MedicalHistory.Contains(medicalHistory));
        }
    }
}