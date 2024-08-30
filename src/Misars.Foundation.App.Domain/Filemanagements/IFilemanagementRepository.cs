using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Misars.Foundation.App.Filemanagements
{
    public partial interface IFilemanagementRepository : IRepository<Filemanagement, Guid>
    {

        Task DeleteAllAsync(
            string? filterText = null,
            string? fileName = null,
            string? filePath = null,
            DateTime? uploadDateMin = null,
            DateTime? uploadDateMax = null,
            CancellationToken cancellationToken = default);
        Task<List<Filemanagement>> GetListAsync(
                    string? filterText = null,
                    string? fileName = null,
                    string? filePath = null,
                    DateTime? uploadDateMin = null,
                    DateTime? uploadDateMax = null,
                    string? sorting = null,
                    int maxResultCount = int.MaxValue,
                    int skipCount = 0,
                    CancellationToken cancellationToken = default
                );

        Task<long> GetCountAsync(
            string? filterText = null,
            string? fileName = null,
            string? filePath = null,
            DateTime? uploadDateMin = null,
            DateTime? uploadDateMax = null,
            CancellationToken cancellationToken = default);
    }
}