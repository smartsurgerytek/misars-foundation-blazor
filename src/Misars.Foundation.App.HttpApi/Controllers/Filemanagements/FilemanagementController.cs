using Asp.Versioning;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Misars.Foundation.App.Filemanagements;
using Volo.Abp.Content;
using Misars.Foundation.App.Shared;

namespace Misars.Foundation.App.Controllers.Filemanagements
{
    [RemoteService]
    [Area("app")]
    [ControllerName("Filemanagement")]
    [Route("api/app/filemanagements")]

    public abstract class FilemanagementControllerBase : AbpController
    {
        protected IFilemanagementsAppService _filemanagementsAppService;

        public FilemanagementControllerBase(IFilemanagementsAppService filemanagementsAppService)
        {
            _filemanagementsAppService = filemanagementsAppService;
        }

        [HttpGet]
        public virtual Task<PagedResultDto<FilemanagementDto>> GetListAsync(GetFilemanagementsInput input)
        {
            return _filemanagementsAppService.GetListAsync(input);
        }

        [HttpGet]
        [Route("{id}")]
        public virtual Task<FilemanagementDto> GetAsync(Guid id)
        {
            return _filemanagementsAppService.GetAsync(id);
        }

        [HttpPost]
        public virtual Task<FilemanagementDto> CreateAsync(FilemanagementCreateDto input)
        {
            return _filemanagementsAppService.CreateAsync(input);
        }

        [HttpPut]
        [Route("{id}")]
        public virtual Task<FilemanagementDto> UpdateAsync(Guid id, FilemanagementUpdateDto input)
        {
            return _filemanagementsAppService.UpdateAsync(id, input);
        }

        [HttpDelete]
        [Route("{id}")]
        public virtual Task DeleteAsync(Guid id)
        {
            return _filemanagementsAppService.DeleteAsync(id);
        }

        [HttpGet]
        [Route("as-excel-file")]
        public virtual Task<IRemoteStreamContent> GetListAsExcelFileAsync(FilemanagementExcelDownloadDto input)
        {
            return _filemanagementsAppService.GetListAsExcelFileAsync(input);
        }

        [HttpGet]
        [Route("download-token")]
        public virtual Task<Misars.Foundation.App.Shared.DownloadTokenResultDto> GetDownloadTokenAsync()
        {
            return _filemanagementsAppService.GetDownloadTokenAsync();
        }

        [HttpDelete]
        [Route("")]
        public virtual Task DeleteByIdsAsync(List<Guid> filemanagementIds)
        {
            return _filemanagementsAppService.DeleteByIdsAsync(filemanagementIds);
        }

        [HttpDelete]
        [Route("all")]
        public virtual Task DeleteAllAsync(GetFilemanagementsInput input)
        {
            return _filemanagementsAppService.DeleteAllAsync(input);
        }
    }
}