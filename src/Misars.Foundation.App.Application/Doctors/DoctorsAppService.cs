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
using Misars.Foundation.App.Doctors;
using MiniExcelLibs;
using Volo.Abp.Content;
using Volo.Abp.Authorization;
using Volo.Abp.Caching;
using Microsoft.Extensions.Caching.Distributed;
using Misars.Foundation.App.Shared;

namespace Misars.Foundation.App.Doctors
{
    [RemoteService(IsEnabled = false)]
    [Authorize(AppPermissions.Doctors.Default)]
    public abstract class DoctorsAppServiceBase : AppAppService
    {
        protected IDistributedCache<DoctorDownloadTokenCacheItem, string> _downloadTokenCache;
        protected IDoctorRepository _doctorRepository;
        protected DoctorManager _doctorManager;

        public DoctorsAppServiceBase(IDoctorRepository doctorRepository, DoctorManager doctorManager, IDistributedCache<DoctorDownloadTokenCacheItem, string> downloadTokenCache)
        {
            _downloadTokenCache = downloadTokenCache;
            _doctorRepository = doctorRepository;
            _doctorManager = doctorManager;

        }

        public virtual async Task<PagedResultDto<DoctorDto>> GetListAsync(GetDoctorsInput input)
        {
            var totalCount = await _doctorRepository.GetCountAsync(input.FilterText, input.name, input.DoctorID, input.Specialty, input.Department);
            var items = await _doctorRepository.GetListAsync(input.FilterText, input.name, input.DoctorID, input.Specialty, input.Department, input.Sorting, input.MaxResultCount, input.SkipCount);

            return new PagedResultDto<DoctorDto>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<Doctor>, List<DoctorDto>>(items)
            };
        }

        public virtual async Task<DoctorDto> GetAsync(Guid id)
        {
            return ObjectMapper.Map<Doctor, DoctorDto>(await _doctorRepository.GetAsync(id));
        }

        [Authorize(AppPermissions.Doctors.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            await _doctorRepository.DeleteAsync(id);
        }

        [Authorize(AppPermissions.Doctors.Create)]
        public virtual async Task<DoctorDto> CreateAsync(DoctorCreateDto input)
        {

            var doctor = await _doctorManager.CreateAsync(
            input.name, input.DoctorID, input.Specialty, input.Department
            );

            return ObjectMapper.Map<Doctor, DoctorDto>(doctor);
        }

        [Authorize(AppPermissions.Doctors.Edit)]
        public virtual async Task<DoctorDto> UpdateAsync(Guid id, DoctorUpdateDto input)
        {

            var doctor = await _doctorManager.UpdateAsync(
            id,
            input.name, input.DoctorID, input.Specialty, input.Department, input.ConcurrencyStamp
            );

            return ObjectMapper.Map<Doctor, DoctorDto>(doctor);
        }

        [AllowAnonymous]
        public virtual async Task<IRemoteStreamContent> GetListAsExcelFileAsync(DoctorExcelDownloadDto input)
        {
            var downloadToken = await _downloadTokenCache.GetAsync(input.DownloadToken);
            if (downloadToken == null || input.DownloadToken != downloadToken.Token)
            {
                throw new AbpAuthorizationException("Invalid download token: " + input.DownloadToken);
            }

            var items = await _doctorRepository.GetListAsync(input.FilterText, input.name, input.DoctorID, input.Specialty, input.Department);

            var memoryStream = new MemoryStream();
            await memoryStream.SaveAsAsync(ObjectMapper.Map<List<Doctor>, List<DoctorExcelDto>>(items));
            memoryStream.Seek(0, SeekOrigin.Begin);

            return new RemoteStreamContent(memoryStream, "Doctors.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        [Authorize(AppPermissions.Doctors.Delete)]
        public virtual async Task DeleteByIdsAsync(List<Guid> doctorIds)
        {
            await _doctorRepository.DeleteManyAsync(doctorIds);
        }

        [Authorize(AppPermissions.Doctors.Delete)]
        public virtual async Task DeleteAllAsync(GetDoctorsInput input)
        {
            await _doctorRepository.DeleteAllAsync(input.FilterText, input.name, input.DoctorID, input.Specialty, input.Department);
        }
        public virtual async Task<Misars.Foundation.App.Shared.DownloadTokenResultDto> GetDownloadTokenAsync()
        {
            var token = Guid.NewGuid().ToString("N");

            await _downloadTokenCache.SetAsync(
                token,
                new DoctorDownloadTokenCacheItem { Token = token },
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