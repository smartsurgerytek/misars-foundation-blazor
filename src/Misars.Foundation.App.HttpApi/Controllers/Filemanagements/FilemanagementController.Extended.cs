using Asp.Versioning;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Misars.Foundation.App.Filemanagements;

namespace Misars.Foundation.App.Controllers.Filemanagements
{
    [RemoteService]
    [Area("app")]
    [ControllerName("Filemanagement")]
    [Route("api/app/filemanagements")]

    public class FilemanagementController : FilemanagementControllerBase, IFilemanagementsAppService
    {
        public FilemanagementController(IFilemanagementsAppService filemanagementsAppService) : base(filemanagementsAppService)
        {
        }
    }
}