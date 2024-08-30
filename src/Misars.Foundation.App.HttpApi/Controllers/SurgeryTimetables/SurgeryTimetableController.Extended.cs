using Asp.Versioning;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Misars.Foundation.App.SurgeryTimetables;

namespace Misars.Foundation.App.Controllers.SurgeryTimetables
{
    [RemoteService]
    [Area("app")]
    [ControllerName("SurgeryTimetable")]
    [Route("api/app/surgery-timetables")]

    public class SurgeryTimetableController : SurgeryTimetableControllerBase, ISurgeryTimetablesAppService
    {
        public SurgeryTimetableController(ISurgeryTimetablesAppService surgeryTimetablesAppService) : base(surgeryTimetablesAppService)
        {
        }
    }
}