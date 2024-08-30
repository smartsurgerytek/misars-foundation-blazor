using Misars.Foundation.App.Shared;
using Misars.Foundation.App.Patients;
using Misars.Foundation.App.Doctors;
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
using Misars.Foundation.App.SurgeryTimetables;
using MiniExcelLibs;
using Volo.Abp.Content;
using Volo.Abp.Authorization;
using Volo.Abp.Caching;
using Microsoft.Extensions.Caching.Distributed;
using Misars.Foundation.App.Shared;

namespace Misars.Foundation.App.SurgeryTimetables
{
    [RemoteService(IsEnabled = false)]
    [Authorize(AppPermissions.SurgeryTimetables.Default)]
    public abstract class SurgeryTimetablesAppServiceBase : AppAppService
    {
        protected IDistributedCache<SurgeryTimetableDownloadTokenCacheItem, string> _downloadTokenCache;
        protected ISurgeryTimetableRepository _surgeryTimetableRepository;
        protected SurgeryTimetableManager _surgeryTimetableManager;

        protected IRepository<Misars.Foundation.App.Doctors.Doctor, Guid> _doctorRepository;
        protected IRepository<Misars.Foundation.App.Patients.Patient, Guid> _patientRepository;

        public SurgeryTimetablesAppServiceBase(ISurgeryTimetableRepository surgeryTimetableRepository, SurgeryTimetableManager surgeryTimetableManager, IDistributedCache<SurgeryTimetableDownloadTokenCacheItem, string> downloadTokenCache, IRepository<Misars.Foundation.App.Doctors.Doctor, Guid> doctorRepository, IRepository<Misars.Foundation.App.Patients.Patient, Guid> patientRepository)
        {
            _downloadTokenCache = downloadTokenCache;
            _surgeryTimetableRepository = surgeryTimetableRepository;
            _surgeryTimetableManager = surgeryTimetableManager; _doctorRepository = doctorRepository;
            _patientRepository = patientRepository;

        }

        public virtual async Task<PagedResultDto<SurgeryTimetableWithNavigationPropertiesDto>> GetListAsync(GetSurgeryTimetablesInput input)
        {
            var totalCount = await _surgeryTimetableRepository.GetCountAsync(input.FilterText, input.startdateMin, input.startdateMax, input.enddateMin, input.enddateMax, input.Time, input.Department, input.Diagnosis, input.SurgicalMethod, input.notes, input.DoctorId, input.PatientId);
            var items = await _surgeryTimetableRepository.GetListWithNavigationPropertiesAsync(input.FilterText, input.startdateMin, input.startdateMax, input.enddateMin, input.enddateMax, input.Time, input.Department, input.Diagnosis, input.SurgicalMethod, input.notes, input.DoctorId, input.PatientId, input.Sorting, input.MaxResultCount, input.SkipCount);

            return new PagedResultDto<SurgeryTimetableWithNavigationPropertiesDto>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<SurgeryTimetableWithNavigationProperties>, List<SurgeryTimetableWithNavigationPropertiesDto>>(items)
            };
        }

        public virtual async Task<SurgeryTimetableWithNavigationPropertiesDto> GetWithNavigationPropertiesAsync(Guid id)
        {
            return ObjectMapper.Map<SurgeryTimetableWithNavigationProperties, SurgeryTimetableWithNavigationPropertiesDto>
                (await _surgeryTimetableRepository.GetWithNavigationPropertiesAsync(id));
        }

        public virtual async Task<SurgeryTimetableDto> GetAsync(Guid id)
        {
            return ObjectMapper.Map<SurgeryTimetable, SurgeryTimetableDto>(await _surgeryTimetableRepository.GetAsync(id));
        }

        public virtual async Task<PagedResultDto<LookupDto<Guid>>> GetDoctorLookupAsync(LookupRequestDto input)
        {
            var query = (await _doctorRepository.GetQueryableAsync())
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                    x => x.name != null &&
                         x.name.Contains(input.Filter));

            var lookupData = await query.PageBy(input.SkipCount, input.MaxResultCount).ToDynamicListAsync<Misars.Foundation.App.Doctors.Doctor>();
            var totalCount = query.Count();
            return new PagedResultDto<LookupDto<Guid>>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<Misars.Foundation.App.Doctors.Doctor>, List<LookupDto<Guid>>>(lookupData)
            };
        }

        public virtual async Task<PagedResultDto<LookupDto<Guid>>> GetPatientLookupAsync(LookupRequestDto input)
        {
            var query = (await _patientRepository.GetQueryableAsync())
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                    x => x.name != null &&
                         x.name.Contains(input.Filter));

            var lookupData = await query.PageBy(input.SkipCount, input.MaxResultCount).ToDynamicListAsync<Misars.Foundation.App.Patients.Patient>();
            var totalCount = query.Count();
            return new PagedResultDto<LookupDto<Guid>>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<Misars.Foundation.App.Patients.Patient>, List<LookupDto<Guid>>>(lookupData)
            };
        }

        [Authorize(AppPermissions.SurgeryTimetables.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            await _surgeryTimetableRepository.DeleteAsync(id);
        }

        [Authorize(AppPermissions.SurgeryTimetables.Create)]
        public virtual async Task<SurgeryTimetableDto> CreateAsync(SurgeryTimetableCreateDto input)
        {

            var surgeryTimetable = await _surgeryTimetableManager.CreateAsync(
            input.DoctorId, input.PatientId, input.startdate, input.enddate, input.Time, input.Department, input.Diagnosis, input.SurgicalMethod, input.notes
            );

            return ObjectMapper.Map<SurgeryTimetable, SurgeryTimetableDto>(surgeryTimetable);
        }

        [Authorize(AppPermissions.SurgeryTimetables.Edit)]
        public virtual async Task<SurgeryTimetableDto> UpdateAsync(Guid id, SurgeryTimetableUpdateDto input)
        {

            var surgeryTimetable = await _surgeryTimetableManager.UpdateAsync(
            id,
            input.DoctorId, input.PatientId, input.startdate, input.enddate, input.Time, input.Department, input.Diagnosis, input.SurgicalMethod, input.notes, input.ConcurrencyStamp
            );

            return ObjectMapper.Map<SurgeryTimetable, SurgeryTimetableDto>(surgeryTimetable);
        }

        [AllowAnonymous]
        public virtual async Task<IRemoteStreamContent> GetListAsExcelFileAsync(SurgeryTimetableExcelDownloadDto input)
        {
            var downloadToken = await _downloadTokenCache.GetAsync(input.DownloadToken);
            if (downloadToken == null || input.DownloadToken != downloadToken.Token)
            {
                throw new AbpAuthorizationException("Invalid download token: " + input.DownloadToken);
            }

            var surgeryTimetables = await _surgeryTimetableRepository.GetListWithNavigationPropertiesAsync(input.FilterText, input.startdateMin, input.startdateMax, input.enddateMin, input.enddateMax, input.Time, input.Department, input.Diagnosis, input.SurgicalMethod, input.notes, input.DoctorId, input.PatientId);
            var items = surgeryTimetables.Select(item => new
            {
                startdate = item.SurgeryTimetable.startdate,
                enddate = item.SurgeryTimetable.enddate,
                Time = item.SurgeryTimetable.Time,
                Department = item.SurgeryTimetable.Department,
                Diagnosis = item.SurgeryTimetable.Diagnosis,
                SurgicalMethod = item.SurgeryTimetable.SurgicalMethod,
                notes = item.SurgeryTimetable.notes,

                Doctor = item.Doctor?.name,
                Patient = item.Patient?.name,

            });

            var memoryStream = new MemoryStream();
            await memoryStream.SaveAsAsync(items);
            memoryStream.Seek(0, SeekOrigin.Begin);

            return new RemoteStreamContent(memoryStream, "SurgeryTimetables.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        [Authorize(AppPermissions.SurgeryTimetables.Delete)]
        public virtual async Task DeleteByIdsAsync(List<Guid> surgerytimetableIds)
        {
            await _surgeryTimetableRepository.DeleteManyAsync(surgerytimetableIds);
        }

        [Authorize(AppPermissions.SurgeryTimetables.Delete)]
        public virtual async Task DeleteAllAsync(GetSurgeryTimetablesInput input)
        {
            await _surgeryTimetableRepository.DeleteAllAsync(input.FilterText, input.startdateMin, input.startdateMax, input.enddateMin, input.enddateMax, input.Time, input.Department, input.Diagnosis, input.SurgicalMethod, input.notes, input.DoctorId, input.PatientId);
        }
        public virtual async Task<Misars.Foundation.App.Shared.DownloadTokenResultDto> GetDownloadTokenAsync()
        {
            var token = Guid.NewGuid().ToString("N");

            await _downloadTokenCache.SetAsync(
                token,
                new SurgeryTimetableDownloadTokenCacheItem { Token = token },
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