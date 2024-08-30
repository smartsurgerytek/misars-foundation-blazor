using Misars.Foundation.App.Shared;
using Asp.Versioning;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Misars.Foundation.App.SurgeryTimetables;
using Volo.Abp.Content;
using Misars.Foundation.App.Shared;

namespace Misars.Foundation.App.Controllers.SurgeryTimetables
{
    [RemoteService]
    [Area("app")]
    [ControllerName("SurgeryTimetable")]
    [Route("api/app/surgery-timetables")]

    public abstract class SurgeryTimetableControllerBase : AbpController
    {
        protected ISurgeryTimetablesAppService _surgeryTimetablesAppService;

        public SurgeryTimetableControllerBase(ISurgeryTimetablesAppService surgeryTimetablesAppService)
        {
            _surgeryTimetablesAppService = surgeryTimetablesAppService;
        }

        [HttpGet]
        public virtual Task<PagedResultDto<SurgeryTimetableWithNavigationPropertiesDto>> GetListAsync(GetSurgeryTimetablesInput input)
        {
            return _surgeryTimetablesAppService.GetListAsync(input);
        }

        [HttpGet]
        [Route("with-navigation-properties/{id}")]
        public virtual Task<SurgeryTimetableWithNavigationPropertiesDto> GetWithNavigationPropertiesAsync(Guid id)
        {
            return _surgeryTimetablesAppService.GetWithNavigationPropertiesAsync(id);
        }

        [HttpGet]
        [Route("{id}")]
        public virtual Task<SurgeryTimetableDto> GetAsync(Guid id)
        {
            return _surgeryTimetablesAppService.GetAsync(id);
        }

        [HttpGet]
        [Route("doctor-lookup")]
        public virtual Task<PagedResultDto<LookupDto<Guid>>> GetDoctorLookupAsync(LookupRequestDto input)
        {
            return _surgeryTimetablesAppService.GetDoctorLookupAsync(input);
        }

        [HttpGet]
        [Route("patient-lookup")]
        public virtual Task<PagedResultDto<LookupDto<Guid>>> GetPatientLookupAsync(LookupRequestDto input)
        {
            return _surgeryTimetablesAppService.GetPatientLookupAsync(input);
        }

        [HttpPost]
        public virtual Task<SurgeryTimetableDto> CreateAsync(SurgeryTimetableCreateDto input)
        {
            return _surgeryTimetablesAppService.CreateAsync(input);
        }

        [HttpPut]
        [Route("{id}")]
        public virtual Task<SurgeryTimetableDto> UpdateAsync(Guid id, SurgeryTimetableUpdateDto input)
        {
            return _surgeryTimetablesAppService.UpdateAsync(id, input);
        }

        [HttpDelete]
        [Route("{id}")]
        public virtual Task DeleteAsync(Guid id)
        {
            return _surgeryTimetablesAppService.DeleteAsync(id);
        }

        [HttpGet]
        [Route("as-excel-file")]
        public virtual Task<IRemoteStreamContent> GetListAsExcelFileAsync(SurgeryTimetableExcelDownloadDto input)
        {
            return _surgeryTimetablesAppService.GetListAsExcelFileAsync(input);
        }

        [HttpGet]
        [Route("download-token")]
        public virtual Task<Misars.Foundation.App.Shared.DownloadTokenResultDto> GetDownloadTokenAsync()
        {
            return _surgeryTimetablesAppService.GetDownloadTokenAsync();
        }

        [HttpDelete]
        [Route("")]
        public virtual Task DeleteByIdsAsync(List<Guid> surgerytimetableIds)
        {
            return _surgeryTimetablesAppService.DeleteByIdsAsync(surgerytimetableIds);
        }

        [HttpDelete]
        [Route("all")]
        public virtual Task DeleteAllAsync(GetSurgeryTimetablesInput input)
        {
            return _surgeryTimetablesAppService.DeleteAllAsync(input);
        }
    }
}