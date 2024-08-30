namespace Misars.Foundation.App.Doctors
{
    public static class DoctorConsts
    {
        private const string DefaultSorting = "{0}name asc";

        public static string GetDefaultSorting(bool withEntityName)
        {
            return string.Format(DefaultSorting, withEntityName ? "Doctor." : string.Empty);
        }

        public const int nameMinLength = 0;
        public const int nameMaxLength = 100;
        public const int DoctorIDMaxLength = 100;
        public const int SpecialtyMaxLength = 100;
        public const int DepartmentMinLength = 0;
        public const int DepartmentMaxLength = 100;
    }
}