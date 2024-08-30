using System;

namespace Misars.Foundation.App.Filemanagements
{
    public abstract class FilemanagementExcelDtoBase
    {
        public string? FileName { get; set; }
        public string? FilePath { get; set; }
        public DateTime UploadDate { get; set; }
    }
}