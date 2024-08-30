using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Misars.Foundation.App.Filemanagements
{
    public abstract class FilemanagementCreateDtoBase
    {
        [StringLength(FilemanagementConsts.FileNameMaxLength, MinimumLength = FilemanagementConsts.FileNameMinLength)]
        public string? FileName { get; set; }
        [StringLength(FilemanagementConsts.FilePathMaxLength, MinimumLength = FilemanagementConsts.FilePathMinLength)]
        public string? FilePath { get; set; }
        public DateTime UploadDate { get; set; }
    }
}