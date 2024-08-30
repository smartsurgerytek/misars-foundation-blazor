using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace Misars.Foundation.App.Filemanagements
{
    public abstract class FilemanagementUpdateDtoBase : IHasConcurrencyStamp
    {
        [StringLength(FilemanagementConsts.FileNameMaxLength, MinimumLength = FilemanagementConsts.FileNameMinLength)]
        public string? FileName { get; set; }
        [StringLength(FilemanagementConsts.FilePathMaxLength, MinimumLength = FilemanagementConsts.FilePathMinLength)]
        public string? FilePath { get; set; }
        public DateTime UploadDate { get; set; }

        public string ConcurrencyStamp { get; set; } = null!;
    }
}