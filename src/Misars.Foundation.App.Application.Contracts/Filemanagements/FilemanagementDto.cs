using System;
using System.Collections.Generic;

using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace Misars.Foundation.App.Filemanagements
{
    public abstract class FilemanagementDtoBase : FullAuditedEntityDto<Guid>, IHasConcurrencyStamp
    {
        public string? FileName { get; set; }
        public string? FilePath { get; set; }
        public DateTime UploadDate { get; set; }

        public string ConcurrencyStamp { get; set; } = null!;

    }
}