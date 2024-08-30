using Asp.Versioning;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Misars.Foundation.App.Patients;
using Volo.Abp.Content;
using Misars.Foundation.App.Shared;

namespace Misars.Foundation.App.Controllers.Patients
{
    [RemoteService]
    [Area("app")]
    [ControllerName("Patient")]
    [Route("api/app/patients")]

    public abstract class PatientControllerBase : AbpController
    {
        protected IPatientsAppService _patientsAppService;

        public PatientControllerBase(IPatientsAppService patientsAppService)
        {
            _patientsAppService = patientsAppService;
        }

        [HttpGet]
        public virtual Task<PagedResultDto<PatientDto>> GetListAsync(GetPatientsInput input)
        {
            return _patientsAppService.GetListAsync(input);
        }

        [HttpGet]
        [Route("{id}")]
        public virtual Task<PatientDto> GetAsync(Guid id)
        {
            return _patientsAppService.GetAsync(id);
        }

        [HttpPost]
        public virtual Task<PatientDto> CreateAsync(PatientCreateDto input)
        {
            return _patientsAppService.CreateAsync(input);
        }

        [HttpPut]
        [Route("{id}")]
        public virtual Task<PatientDto> UpdateAsync(Guid id, PatientUpdateDto input)
        {
            return _patientsAppService.UpdateAsync(id, input);
        }

        [HttpDelete]
        [Route("{id}")]
        public virtual Task DeleteAsync(Guid id)
        {
            return _patientsAppService.DeleteAsync(id);
        }

        [HttpGet]
        [Route("as-excel-file")]
        public virtual Task<IRemoteStreamContent> GetListAsExcelFileAsync(PatientExcelDownloadDto input)
        {
            return _patientsAppService.GetListAsExcelFileAsync(input);
        }

        [HttpGet]
        [Route("download-token")]
        public virtual Task<Misars.Foundation.App.Shared.DownloadTokenResultDto> GetDownloadTokenAsync()
        {
            return _patientsAppService.GetDownloadTokenAsync();
        }

        [HttpDelete]
        [Route("")]
        public virtual Task DeleteByIdsAsync(List<Guid> patientIds)
        {
            return _patientsAppService.DeleteByIdsAsync(patientIds);
        }

        [HttpDelete]
        [Route("all")]
        public virtual Task DeleteAllAsync(GetPatientsInput input)
        {
            return _patientsAppService.DeleteAllAsync(input);
        }
    }
}