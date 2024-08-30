using Misars.Foundation.App.Doctors;
using Misars.Foundation.App.Patients;

using System;
using Volo.Abp.Application.Dtos;
using System.Collections.Generic;

namespace Misars.Foundation.App.SurgeryTimetables
{
    public abstract class SurgeryTimetableWithNavigationPropertiesDtoBase
    {
        public SurgeryTimetableDto SurgeryTimetable { get; set; } = null!;

        public DoctorDto Doctor { get; set; } = null!;
        public PatientDto Patient { get; set; } = null!;

    }
}