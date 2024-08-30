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

namespace Misars.Foundation.App.Filemanagements
{
    public abstract class EfCoreFilemanagementRepositoryBase : EfCoreRepository<AppDbContext, Filemanagement, Guid>
    {
        public EfCoreFilemanagementRepositoryBase(IDbContextProvider<AppDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public virtual async Task DeleteAllAsync(
            string? filterText = null,
                        string? fileName = null,
            string? filePath = null,
            DateTime? uploadDateMin = null,
            DateTime? uploadDateMax = null,
            CancellationToken cancellationToken = default)
        {

            var query = await GetQueryableAsync();

            query = ApplyFilter(query, filterText, fileName, filePath, uploadDateMin, uploadDateMax);

            var ids = query.Select(x => x.Id);
            await DeleteManyAsync(ids, cancellationToken: GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<Filemanagement>> GetListAsync(
            string? filterText = null,
            string? fileName = null,
            string? filePath = null,
            DateTime? uploadDateMin = null,
            DateTime? uploadDateMax = null,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetQueryableAsync()), filterText, fileName, filePath, uploadDateMin, uploadDateMax);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? FilemanagementConsts.GetDefaultSorting(false) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        public virtual async Task<long> GetCountAsync(
            string? filterText = null,
            string? fileName = null,
            string? filePath = null,
            DateTime? uploadDateMin = null,
            DateTime? uploadDateMax = null,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetDbSetAsync()), filterText, fileName, filePath, uploadDateMin, uploadDateMax);
            return await query.LongCountAsync(GetCancellationToken(cancellationToken));
        }

        protected virtual IQueryable<Filemanagement> ApplyFilter(
            IQueryable<Filemanagement> query,
            string? filterText = null,
            string? fileName = null,
            string? filePath = null,
            DateTime? uploadDateMin = null,
            DateTime? uploadDateMax = null)
        {
            return query
                    .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.FileName!.Contains(filterText!) || e.FilePath!.Contains(filterText!))
                    .WhereIf(!string.IsNullOrWhiteSpace(fileName), e => e.FileName.Contains(fileName))
                    .WhereIf(!string.IsNullOrWhiteSpace(filePath), e => e.FilePath.Contains(filePath))
                    .WhereIf(uploadDateMin.HasValue, e => e.UploadDate >= uploadDateMin!.Value)
                    .WhereIf(uploadDateMax.HasValue, e => e.UploadDate <= uploadDateMax!.Value);
        }
    }
}