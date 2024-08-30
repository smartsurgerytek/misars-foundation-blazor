namespace Misars.Foundation.App.Patients
{
    public static class PatientConsts
    {
        private const string DefaultSorting = "{0}name asc";

        public static string GetDefaultSorting(bool withEntityName)
        {
            return string.Format(DefaultSorting, withEntityName ? "Patient." : string.Empty);
        }

        public const int nameMinLength = 0;
        public const int nameMaxLength = 100;
        public const int PatientIDMinLength = 0;
        public const int PatientIDMaxLength = 100;
        public const int GenderMinLength = 0;
        public const int GenderMaxLength = 100;
        public const int MedicalHistoryMinLength = 0;
        public const int MedicalHistoryMaxLength = 100;
    }
}