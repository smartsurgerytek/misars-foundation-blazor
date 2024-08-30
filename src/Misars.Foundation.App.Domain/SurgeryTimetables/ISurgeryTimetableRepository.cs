using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Misars.Foundation.App.SurgeryTimetables
{
    public partial interface ISurgeryTimetableRepository : IRepository<SurgeryTimetable, Guid>
    {

        Task DeleteAllAsync(
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
            CancellationToken cancellationToken = default);
        Task<SurgeryTimetableWithNavigationProperties> GetWithNavigationPropertiesAsync(
            Guid id,
            CancellationToken cancellationToken = default
        );

        Task<List<SurgeryTimetableWithNavigationProperties>> GetListWithNavigationPropertiesAsync(
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
            CancellationToken cancellationToken = default
        );

        Task<List<SurgeryTimetable>> GetListAsync(
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
                    CancellationToken cancellationToken = default
                );

        Task<long> GetCountAsync(
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
            CancellationToken cancellationToken = default);
    }
}