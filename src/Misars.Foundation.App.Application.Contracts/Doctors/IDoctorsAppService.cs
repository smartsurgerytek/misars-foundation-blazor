using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;
using Misars.Foundation.App.Shared;

namespace Misars.Foundation.App.Doctors
{
    public partial interface IDoctorsAppService : IApplicationService
    {

        Task<PagedResultDto<DoctorDto>> GetListAsync(GetDoctorsInput input);

        Task<DoctorDto> GetAsync(Guid id);

        Task DeleteAsync(Guid id);

        Task<DoctorDto> CreateAsync(DoctorCreateDto input);

        Task<DoctorDto> UpdateAsync(Guid id, DoctorUpdateDto input);

        Task<IRemoteStreamContent> GetListAsExcelFileAsync(DoctorExcelDownloadDto input);
        Task DeleteByIdsAsync(List<Guid> doctorIds);

        Task DeleteAllAsync(GetDoctorsInput input);
        Task<Misars.Foundation.App.Shared.DownloadTokenResultDto> GetDownloadTokenAsync();

    }
}