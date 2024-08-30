using Asp.Versioning;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Misars.Foundation.App.Patients;

namespace Misars.Foundation.App.Controllers.Patients
{
    [RemoteService]
    [Area("app")]
    [ControllerName("Patient")]
    [Route("api/app/patients")]

    public class PatientController : PatientControllerBase, IPatientsAppService
    {
        public PatientController(IPatientsAppService patientsAppService) : base(patientsAppService)
        {
        }
    }
}