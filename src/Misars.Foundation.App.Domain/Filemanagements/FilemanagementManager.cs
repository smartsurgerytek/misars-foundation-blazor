using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Data;

namespace Misars.Foundation.App.Filemanagements
{
    public abstract class FilemanagementManagerBase : DomainService
    {
        protected IFilemanagementRepository _filemanagementRepository;

        public FilemanagementManagerBase(IFilemanagementRepository filemanagementRepository)
        {
            _filemanagementRepository = filemanagementRepository;
        }

        public virtual async Task<Filemanagement> CreateAsync(
        DateTime uploadDate, string? fileName = null, string? filePath = null)
        {
            Check.NotNull(uploadDate, nameof(uploadDate));
            Check.Length(fileName, nameof(fileName), FilemanagementConsts.FileNameMaxLength, FilemanagementConsts.FileNameMinLength);
            Check.Length(filePath, nameof(filePath), FilemanagementConsts.FilePathMaxLength, FilemanagementConsts.FilePathMinLength);

            var filemanagement = new Filemanagement(
             GuidGenerator.Create(),
             uploadDate, fileName, filePath
             );

            return await _filemanagementRepository.InsertAsync(filemanagement);
        }

        public virtual async Task<Filemanagement> UpdateAsync(
            Guid id,
            DateTime uploadDate, string? fileName = null, string? filePath = null, [CanBeNull] string? concurrencyStamp = null
        )
        {
            Check.NotNull(uploadDate, nameof(uploadDate));
            Check.Length(fileName, nameof(fileName), FilemanagementConsts.FileNameMaxLength, FilemanagementConsts.FileNameMinLength);
            Check.Length(filePath, nameof(filePath), FilemanagementConsts.FilePathMaxLength, FilemanagementConsts.FilePathMinLength);

            var filemanagement = await _filemanagementRepository.GetAsync(id);

            filemanagement.UploadDate = uploadDate;
            filemanagement.FileName = fileName;
            filemanagement.FilePath = filePath;

            filemanagement.SetConcurrencyStampIfNotNull(concurrencyStamp);
            return await _filemanagementRepository.UpdateAsync(filemanagement);
        }

    }
}