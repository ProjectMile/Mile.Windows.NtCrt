using Mile.DotNet.Helpers;

namespace Mile.NtCrt.GeneratorCore
{
    public class ProjectPath
    {
        public static readonly string RepositoryRoot = Git.GetRootPath();

        public static readonly string ReferencesRoot =
            RepositoryRoot + @"\Mile.NtCrt.References";

        public static string GetPath(
            string Category,
            string Version,
            string Platform,
            string FileName)
        {
            return string.Format(
                @"{0}\{1}\{2}\{3}\{4}",
                ReferencesRoot,
                Category,
                Version,
                Platform,
                FileName);
        }

        public static string GetLibraryPath(
            string Version,
            string Platform,
            string FileName)
        {
            return GetPath("Lib", Version, Platform, FileName);
        }
    }
}
