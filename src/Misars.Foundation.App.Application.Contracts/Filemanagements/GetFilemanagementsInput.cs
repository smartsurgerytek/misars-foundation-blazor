using Volo.Abp.Application.Dtos;
using System;

namespace Misars.Foundation.App.Filemanagements
{
    public abstract class GetFilemanagementsInputBase : PagedAndSortedResultRequestDto
    {

        public string? FilterText { get; set; }

        public string? FileName { get; set; }
        public string? FilePath { get; set; }
        public DateTime? UploadDateMin { get; set; }
        public DateTime? UploadDateMax { get; set; }

        public GetFilemanagementsInputBase()
        {

        }
    }
}