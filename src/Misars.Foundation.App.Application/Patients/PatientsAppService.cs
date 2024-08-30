using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Misars.Foundation.App.Permissions;
using Misars.Foundation.App.Patients;
using MiniExcelLibs;
using Volo.Abp.Content;
using Volo.Abp.Authorization;
using Volo.Abp.Caching;
using Microsoft.Extensions.Caching.Distributed;
using Misars.Foundation.App.Shared;

namespace Misars.Foundation.App.Patients
{
    [RemoteService(IsEnabled = false)]
    [Authorize(AppPermissions.Patients.Default)]
    public abstract class PatientsAppServiceBase : AppAppService
    {
        protected IDistributedCache<PatientDownloadTokenCacheItem, string> _downloadTokenCache;
        protected IPatientRepository _patientRepository;
        protected PatientManager _patientManager;

        public PatientsAppServiceBase(IPatientRepository patientRepository, PatientManager patientManager, IDistributedCache<PatientDownloadTokenCacheItem, string> downloadTokenCache)
        {
            _downloadTokenCache = downloadTokenCache;
            _patientRepository = patientRepository;
            _patientManager = patientManager;

        }

        public virtual async Task<PagedResultDto<PatientDto>> GetListAsync(GetPatientsInput input)
        {
            var totalCount = await _patientRepository.GetCountAsync(input.FilterText, input.name, input.PatientID, input.DateofBirthMin, input.DateofBirthMax, input.Gender, input.MedicalHistory);
            var items = await _patientRepository.GetListAsync(input.FilterText, input.name, input.PatientID, input.DateofBirthMin, input.DateofBirthMax, input.Gender, input.MedicalHistory, input.Sorting, input.MaxResultCount, input.SkipCount);

            return new PagedResultDto<PatientDto>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<Patient>, List<PatientDto>>(items)
            };
        }

        public virtual async Task<PatientDto> GetAsync(Guid id)
        {
            return ObjectMapper.Map<Patient, PatientDto>(await _patientRepository.GetAsync(id));
        }

        [Authorize(AppPermissions.Patients.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            await _patientRepository.DeleteAsync(id);
        }

        [Authorize(AppPermissions.Patients.Create)]
        public virtual async Task<PatientDto> CreateAsync(PatientCreateDto input)
        {

            var patient = await _patientManager.CreateAsync(
            input.DateofBirth, input.name, input.PatientID, input.Gender, input.MedicalHistory
            );

            return ObjectMapper.Map<Patient, PatientDto>(patient);
        }

        [Authorize(AppPermissions.Patients.Edit)]
        public virtual async Task<PatientDto> UpdateAsync(Guid id, PatientUpdateDto input)
        {

            var patient = await _patientManager.UpdateAsync(
            id,
            input.DateofBirth, input.name, input.PatientID, input.Gender, input.MedicalHistory, input.ConcurrencyStamp
            );

            return ObjectMapper.Map<Patient, PatientDto>(patient);
        }

        [AllowAnonymous]
        public virtual async Task<IRemoteStreamContent> GetListAsExcelFileAsync(PatientExcelDownloadDto input)
        {
            var downloadToken = await _downloadTokenCache.GetAsync(input.DownloadToken);
            if (downloadToken == null || input.DownloadToken != downloadToken.Token)
            {
                throw new AbpAuthorizationException("Invalid download token: " + input.DownloadToken);
            }

            var items = await _patientRepository.GetListAsync(input.FilterText, input.name, input.PatientID, input.DateofBirthMin, input.DateofBirthMax, input.Gender, input.MedicalHistory);

            var memoryStream = new MemoryStream();
            await memoryStream.SaveAsAsync(ObjectMapper.Map<List<Patient>, List<PatientExcelDto>>(items));
            memoryStream.Seek(0, SeekOrigin.Begin);

            return new RemoteStreamContent(memoryStream, "Patients.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        [Authorize(AppPermissions.Patients.Delete)]
        public virtual async Task DeleteByIdsAsync(List<Guid> patientIds)
        {
            await _patientRepository.DeleteManyAsync(patientIds);
        }

        [Authorize(AppPermissions.Patients.Delete)]
        public virtual async Task DeleteAllAsync(GetPatientsInput input)
        {
            await _patientRepository.DeleteAllAsync(input.FilterText, input.name, input.PatientID, input.DateofBirthMin, input.DateofBirthMax, input.Gender, input.MedicalHistory);
        }
        public virtual async Task<Misars.Foundation.App.Shared.DownloadTokenResultDto> GetDownloadTokenAsync()
        {
            var token = Guid.NewGuid().ToString("N");

            await _downloadTokenCache.SetAsync(
                token,
                new PatientDownloadTokenCacheItem { Token = token },
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
                });

            return new Misars.Foundation.App.Shared.DownloadTokenResultDto
            {
                Token = token
            };
        }
    }
}