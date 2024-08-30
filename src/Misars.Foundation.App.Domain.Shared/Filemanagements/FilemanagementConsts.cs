namespace Misars.Foundation.App.Filemanagements
{
    public static class FilemanagementConsts
    {
        private const string DefaultSorting = "{0}FileName asc";

        public static string GetDefaultSorting(bool withEntityName)
        {
            return string.Format(DefaultSorting, withEntityName ? "Filemanagement." : string.Empty);
        }

        public const int FileNameMinLength = 0;
        public const int FileNameMaxLength = 100;
        public const int FilePathMinLength = 0;
        public const int FilePathMaxLength = 100;
    }
}