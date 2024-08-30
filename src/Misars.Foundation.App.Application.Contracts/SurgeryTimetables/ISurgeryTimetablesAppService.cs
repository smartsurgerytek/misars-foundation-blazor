using Misars.Foundation.App.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;
using Misars.Foundation.App.Shared;

namespace Misars.Foundation.App.SurgeryTimetables
{
    public partial interface ISurgeryTimetablesAppService : IApplicationService
    {

        Task<PagedResultDto<SurgeryTimetableWithNavigationPropertiesDto>> GetListAsync(GetSurgeryTimetablesInput input);

        Task<SurgeryTimetableWithNavigationPropertiesDto> GetWithNavigationPropertiesAsync(Guid id);

        Task<SurgeryTimetableDto> GetAsync(Guid id);

        Task<PagedResultDto<LookupDto<Guid>>> GetDoctorLookupAsync(LookupRequestDto input);

        Task<PagedResultDto<LookupDto<Guid>>> GetPatientLookupAsync(LookupRequestDto input);

        Task DeleteAsync(Guid id);

        Task<SurgeryTimetableDto> CreateAsync(SurgeryTimetableCreateDto input);

        Task<SurgeryTimetableDto> UpdateAsync(Guid id, SurgeryTimetableUpdateDto input);

        Task<IRemoteStreamContent> GetListAsExcelFileAsync(SurgeryTimetableExcelDownloadDto input);
        Task DeleteByIdsAsync(List<Guid> surgerytimetableIds);

        Task DeleteAllAsync(GetSurgeryTimetablesInput input);
        Task<Misars.Foundation.App.Shared.DownloadTokenResultDto> GetDownloadTokenAsync();

    }
}