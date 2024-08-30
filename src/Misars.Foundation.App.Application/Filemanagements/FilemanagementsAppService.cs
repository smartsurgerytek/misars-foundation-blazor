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
using Misars.Foundation.App.Filemanagements;
using MiniExcelLibs;
using Volo.Abp.Content;
using Volo.Abp.Authorization;
using Volo.Abp.Caching;
using Microsoft.Extensions.Caching.Distributed;
using Misars.Foundation.App.Shared;

namespace Misars.Foundation.App.Filemanagements
{
    [RemoteService(IsEnabled = false)]
    [Authorize(AppPermissions.Filemanagements.Default)]
    public abstract class FilemanagementsAppServiceBase : AppAppService
    {
        protected IDistributedCache<FilemanagementDownloadTokenCacheItem, string> _downloadTokenCache;
        protected IFilemanagementRepository _filemanagementRepository;
        protected FilemanagementManager _filemanagementManager;

        public FilemanagementsAppServiceBase(IFilemanagementRepository filemanagementRepository, FilemanagementManager filemanagementManager, IDistributedCache<FilemanagementDownloadTokenCacheItem, string> downloadTokenCache)
        {
            _downloadTokenCache = downloadTokenCache;
            _filemanagementRepository = filemanagementRepository;
            _filemanagementManager = filemanagementManager;

        }

        public virtual async Task<PagedResultDto<FilemanagementDto>> GetListAsync(GetFilemanagementsInput input)
        {
            var totalCount = await _filemanagementRepository.GetCountAsync(input.FilterText, input.FileName, input.FilePath, input.UploadDateMin, input.UploadDateMax);
            var items = await _filemanagementRepository.GetListAsync(input.FilterText, input.FileName, input.FilePath, input.UploadDateMin, input.UploadDateMax, input.Sorting, input.MaxResultCount, input.SkipCount);

            return new PagedResultDto<FilemanagementDto>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<Filemanagement>, List<FilemanagementDto>>(items)
            };
        }

        public virtual async Task<FilemanagementDto> GetAsync(Guid id)
        {
            return ObjectMapper.Map<Filemanagement, FilemanagementDto>(await _filemanagementRepository.GetAsync(id));
        }

        [Authorize(AppPermissions.Filemanagements.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            await _filemanagementRepository.DeleteAsync(id);
        }

        [Authorize(AppPermissions.Filemanagements.Create)]
        public virtual async Task<FilemanagementDto> CreateAsync(FilemanagementCreateDto input)
        {

            var filemanagement = await _filemanagementManager.CreateAsync(
            input.UploadDate, input.FileName, input.FilePath
            );

            return ObjectMapper.Map<Filemanagement, FilemanagementDto>(filemanagement);
        }

        [Authorize(AppPermissions.Filemanagements.Edit)]
        public virtual async Task<FilemanagementDto> UpdateAsync(Guid id, FilemanagementUpdateDto input)
        {

            var filemanagement = await _filemanagementManager.UpdateAsync(
            id,
            input.UploadDate, input.FileName, input.FilePath, input.ConcurrencyStamp
            );

            return ObjectMapper.Map<Filemanagement, FilemanagementDto>(filemanagement);
        }

        [AllowAnonymous]
        public virtual async Task<IRemoteStreamContent> GetListAsExcelFileAsync(FilemanagementExcelDownloadDto input)
        {
            var downloadToken = await _downloadTokenCache.GetAsync(input.DownloadToken);
            if (downloadToken == null || input.DownloadToken != downloadToken.Token)
            {
                throw new AbpAuthorizationException("Invalid download token: " + input.DownloadToken);
            }

            var items = await _filemanagementRepository.GetListAsync(input.FilterText, input.FileName, input.FilePath, input.UploadDateMin, input.UploadDateMax);

            var memoryStream = new MemoryStream();
            await memoryStream.SaveAsAsync(ObjectMapper.Map<List<Filemanagement>, List<FilemanagementExcelDto>>(items));
            memoryStream.Seek(0, SeekOrigin.Begin);

            return new RemoteStreamContent(memoryStream, "Filemanagements.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        [Authorize(AppPermissions.Filemanagements.Delete)]
        public virtual async Task DeleteByIdsAsync(List<Guid> filemanagementIds)
        {
            await _filemanagementRepository.DeleteManyAsync(filemanagementIds);
        }

        [Authorize(AppPermissions.Filemanagements.Delete)]
        public virtual async Task DeleteAllAsync(GetFilemanagementsInput input)
        {
            await _filemanagementRepository.DeleteAllAsync(input.FilterText, input.FileName, input.FilePath, input.UploadDateMin, input.UploadDateMax);
        }
        public virtual async Task<Misars.Foundation.App.Shared.DownloadTokenResultDto> GetDownloadTokenAsync()
        {
            var token = Guid.NewGuid().ToString("N");

            await _downloadTokenCache.SetAsync(
                token,
                new FilemanagementDownloadTokenCacheItem { Token = token },
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