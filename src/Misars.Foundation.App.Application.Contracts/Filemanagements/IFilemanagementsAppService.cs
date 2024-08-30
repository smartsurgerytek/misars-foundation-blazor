using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;
using Misars.Foundation.App.Shared;

namespace Misars.Foundation.App.Filemanagements
{
    public partial interface IFilemanagementsAppService : IApplicationService
    {

        Task<PagedResultDto<FilemanagementDto>> GetListAsync(GetFilemanagementsInput input);

        Task<FilemanagementDto> GetAsync(Guid id);

        Task DeleteAsync(Guid id);

        Task<FilemanagementDto> CreateAsync(FilemanagementCreateDto input);

        Task<FilemanagementDto> UpdateAsync(Guid id, FilemanagementUpdateDto input);

        Task<IRemoteStreamContent> GetListAsExcelFileAsync(FilemanagementExcelDownloadDto input);
        Task DeleteByIdsAsync(List<Guid> filemanagementIds);

        Task DeleteAllAsync(GetFilemanagementsInput input);
        Task<Misars.Foundation.App.Shared.DownloadTokenResultDto> GetDownloadTokenAsync();

    }
}