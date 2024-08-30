using Asp.Versioning;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Misars.Foundation.App.Doctors;

namespace Misars.Foundation.App.Controllers.Doctors
{
    [RemoteService]
    [Area("app")]
    [ControllerName("Doctor")]
    [Route("api/app/doctors")]

    public class DoctorController : DoctorControllerBase, IDoctorsAppService
    {
        public DoctorController(IDoctorsAppService doctorsAppService) : base(doctorsAppService)
        {
        }
    }
}