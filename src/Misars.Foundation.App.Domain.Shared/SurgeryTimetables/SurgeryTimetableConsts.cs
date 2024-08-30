namespace Misars.Foundation.App.SurgeryTimetables
{
    public static class SurgeryTimetableConsts
    {
        private const string DefaultSorting = "{0}startdate asc";

        public static string GetDefaultSorting(bool withEntityName)
        {
            return string.Format(DefaultSorting, withEntityName ? "SurgeryTimetable." : string.Empty);
        }

        public const int TimeMinLength = 0;
        public const int TimeMaxLength = 100;
        public const int DiagnosisMinLength = 0;
        public const int DiagnosisMaxLength = 100;
        public const int SurgicalMethodMinLength = 0;
        public const int SurgicalMethodMaxLength = 100;
        public const int notesMinLength = 0;
        public const int notesMaxLength = 100;
    }
}