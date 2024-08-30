using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;
using Misars.Foundation.App.Shared;

namespace Misars.Foundation.App.Patients
{
    public partial interface IPatientsAppService : IApplicationService
    {

        Task<PagedResultDto<PatientDto>> GetListAsync(GetPatientsInput input);

        Task<PatientDto> GetAsync(Guid id);

        Task DeleteAsync(Guid id);

        Task<PatientDto> CreateAsync(PatientCreateDto input);

        Task<PatientDto> UpdateAsync(Guid id, PatientUpdateDto input);

        Task<IRemoteStreamContent> GetListAsExcelFileAsync(PatientExcelDownloadDto input);
        Task DeleteByIdsAsync(List<Guid> patientIds);

        Task DeleteAllAsync(GetPatientsInput input);
        Task<Misars.Foundation.App.Shared.DownloadTokenResultDto> GetDownloadTokenAsync();

    }
}