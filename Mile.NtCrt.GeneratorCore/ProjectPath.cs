using Mile.Project.Helpers;

namespace Mile.NtCrt.GeneratorCore
{
    public class ProjectPath
    {
        public static readonly string RepositoryRoot =
            GitRepository.GetRootPath();

        public static readonly string ReferencesRoot =
            RepositoryRoot + @"\Mile.NtCrt.References";
    }
}
