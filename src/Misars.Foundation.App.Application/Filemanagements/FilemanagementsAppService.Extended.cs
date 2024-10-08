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
    public class FilemanagementsAppService : FilemanagementsAppServiceBase, IFilemanagementsAppService
    {
        //<suite-custom-code-autogenerated>
        public FilemanagementsAppService(IFilemanagementRepository filemanagementRepository, FilemanagementManager filemanagementManager, IDistributedCache<FilemanagementDownloadTokenCacheItem, string> downloadTokenCache)
            : base(filemanagementRepository, filemanagementManager, downloadTokenCache)
        {
        }
        //</suite-custom-code-autogenerated>

        //Write your custom code...
    }
}