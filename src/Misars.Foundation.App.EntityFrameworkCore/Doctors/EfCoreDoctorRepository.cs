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

namespace Misars.Foundation.App.Doctors
{
    public abstract class EfCoreDoctorRepositoryBase : EfCoreRepository<AppDbContext, Doctor, Guid>
    {
        public EfCoreDoctorRepositoryBase(IDbContextProvider<AppDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public virtual async Task DeleteAllAsync(
            string? filterText = null,
                        string? name = null,
            string? doctorID = null,
            string? specialty = null,
            string? department = null,
            CancellationToken cancellationToken = default)
        {

            var query = await GetQueryableAsync();

            query = ApplyFilter(query, filterText, name, doctorID, specialty, department);

            var ids = query.Select(x => x.Id);
            await DeleteManyAsync(ids, cancellationToken: GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<Doctor>> GetListAsync(
            string? filterText = null,
            string? name = null,
            string? doctorID = null,
            string? specialty = null,
            string? department = null,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetQueryableAsync()), filterText, name, doctorID, specialty, department);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? DoctorConsts.GetDefaultSorting(false) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        public virtual async Task<long> GetCountAsync(
            string? filterText = null,
            string? name = null,
            string? doctorID = null,
            string? specialty = null,
            string? department = null,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetDbSetAsync()), filterText, name, doctorID, specialty, department);
            return await query.LongCountAsync(GetCancellationToken(cancellationToken));
        }

        protected virtual IQueryable<Doctor> ApplyFilter(
            IQueryable<Doctor> query,
            string? filterText = null,
            string? name = null,
            string? doctorID = null,
            string? specialty = null,
            string? department = null)
        {
            return query
                    .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.name!.Contains(filterText!) || e.DoctorID!.Contains(filterText!) || e.Specialty!.Contains(filterText!) || e.Department!.Contains(filterText!))
                    .WhereIf(!string.IsNullOrWhiteSpace(name), e => e.name.Contains(name))
                    .WhereIf(!string.IsNullOrWhiteSpace(doctorID), e => e.DoctorID.Contains(doctorID))
                    .WhereIf(!string.IsNullOrWhiteSpace(specialty), e => e.Specialty.Contains(specialty))
                    .WhereIf(!string.IsNullOrWhiteSpace(department), e => e.Department.Contains(department));
        }
    }
}