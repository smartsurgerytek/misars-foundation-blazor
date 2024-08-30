using Asp.Versioning;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Misars.Foundation.App.Doctors;
using Volo.Abp.Content;
using Misars.Foundation.App.Shared;

namespace Misars.Foundation.App.Controllers.Doctors
{
    [RemoteService]
    [Area("app")]
    [ControllerName("Doctor")]
    [Route("api/app/doctors")]

    public abstract class DoctorControllerBase : AbpController
    {
        protected IDoctorsAppService _doctorsAppService;

        public DoctorControllerBase(IDoctorsAppService doctorsAppService)
        {
            _doctorsAppService = doctorsAppService;
        }

        [HttpGet]
        public virtual Task<PagedResultDto<DoctorDto>> GetListAsync(GetDoctorsInput input)
        {
            return _doctorsAppService.GetListAsync(input);
        }

        [HttpGet]
        [Route("{id}")]
        public virtual Task<DoctorDto> GetAsync(Guid id)
        {
            return _doctorsAppService.GetAsync(id);
        }

        [HttpPost]
        public virtual Task<DoctorDto> CreateAsync(DoctorCreateDto input)
        {
            return _doctorsAppService.CreateAsync(input);
        }

        [HttpPut]
        [Route("{id}")]
        public virtual Task<DoctorDto> UpdateAsync(Guid id, DoctorUpdateDto input)
        {
            return _doctorsAppService.UpdateAsync(id, input);
        }

        [HttpDelete]
        [Route("{id}")]
        public virtual Task DeleteAsync(Guid id)
        {
            return _doctorsAppService.DeleteAsync(id);
        }

        [HttpGet]
        [Route("as-excel-file")]
        public virtual Task<IRemoteStreamContent> GetListAsExcelFileAsync(DoctorExcelDownloadDto input)
        {
            return _doctorsAppService.GetListAsExcelFileAsync(input);
        }

        [HttpGet]
        [Route("download-token")]
        public virtual Task<Misars.Foundation.App.Shared.DownloadTokenResultDto> GetDownloadTokenAsync()
        {
            return _doctorsAppService.GetDownloadTokenAsync();
        }

        [HttpDelete]
        [Route("")]
        public virtual Task DeleteByIdsAsync(List<Guid> doctorIds)
        {
            return _doctorsAppService.DeleteByIdsAsync(doctorIds);
        }

        [HttpDelete]
        [Route("all")]
        public virtual Task DeleteAllAsync(GetDoctorsInput input)
        {
            return _doctorsAppService.DeleteAllAsync(input);
        }
    }
}