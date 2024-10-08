using Misars.Foundation.App.Doctors;
using Misars.Foundation.App.Patients;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using JetBrains.Annotations;

using Volo.Abp;

namespace Misars.Foundation.App.SurgeryTimetables
{
    public class SurgeryTimetable : SurgeryTimetableBase
    {
        //<suite-custom-code-autogenerated>
        protected SurgeryTimetable()
        {

        }

        public SurgeryTimetable(Guid id, Guid? doctorId, Guid? patientId, DateTime startdate, DateTime enddate, string? time = null, string? department = null, string? diagnosis = null, string? surgicalMethod = null, string? notes = null)
            : base(id, doctorId, patientId, startdate, enddate, time, department, diagnosis, surgicalMethod, notes)
        {
        }
        //</suite-custom-code-autogenerated>

        //Write your custom code...
    }
}