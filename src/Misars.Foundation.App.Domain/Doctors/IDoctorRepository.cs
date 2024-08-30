using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Misars.Foundation.App.Doctors
{
    public partial interface IDoctorRepository : IRepository<Doctor, Guid>
    {

        Task DeleteAllAsync(
            string? filterText = null,
            string? name = null,
            string? doctorID = null,
            string? specialty = null,
            string? department = null,
            CancellationToken cancellationToken = default);
        Task<List<Doctor>> GetListAsync(
                    string? filterText = null,
                    string? name = null,
                    string? doctorID = null,
                    string? specialty = null,
                    string? department = null,
                    string? sorting = null,
                    int maxResultCount = int.MaxValue,
                    int skipCount = 0,
                    CancellationToken cancellationToken = default
                );

        Task<long> GetCountAsync(
            string? filterText = null,
            string? name = null,
            string? doctorID = null,
            string? specialty = null,
            string? department = null,
            CancellationToken cancellationToken = default);
    }
}