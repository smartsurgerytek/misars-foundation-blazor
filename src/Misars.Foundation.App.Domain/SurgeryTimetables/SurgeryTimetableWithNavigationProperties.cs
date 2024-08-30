using Misars.Foundation.App.Doctors;
using Misars.Foundation.App.Patients;

using System;
using System.Collections.Generic;

namespace Misars.Foundation.App.SurgeryTimetables
{
    public abstract class SurgeryTimetableWithNavigationPropertiesBase
    {
        public SurgeryTimetable SurgeryTimetable { get; set; } = null!;

        public Doctor Doctor { get; set; } = null!;
        public Patient Patient { get; set; } = null!;
        

        
    }
}