using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using JetBrains.Annotations;

using Volo.Abp;

namespace Misars.Foundation.App.Filemanagements
{
    public abstract class FilemanagementBase : FullAuditedAggregateRoot<Guid>
    {
        [CanBeNull]
        public virtual string? FileName { get; set; }

        [CanBeNull]
        public virtual string? FilePath { get; set; }

        public virtual DateTime UploadDate { get; set; }

        protected FilemanagementBase()
        {

        }

        public FilemanagementBase(Guid id, DateTime uploadDate, string? fileName = null, string? filePath = null)
        {

            Id = id;
            Check.Length(fileName, nameof(fileName), FilemanagementConsts.FileNameMaxLength, FilemanagementConsts.FileNameMinLength);
            Check.Length(filePath, nameof(filePath), FilemanagementConsts.FilePathMaxLength, FilemanagementConsts.FilePathMinLength);
            UploadDate = uploadDate;
            FileName = fileName;
            FilePath = filePath;
        }

    }
}