using Mile.Project.Helpers;

namespace Mile.NtCrt.UserModeCrtLibraryGenerator
{
    internal class Program
    {
        private static readonly string ProjectRootPath =
           GitRepository.GetRootPath();

        private static readonly string ReferencesRootPath =
            ProjectRootPath + @"\Mile.NtCrt.References";

        private static readonly string[] SupportedPlatforms =
        {
            "arm64",
            "x64",
            "x86"
        };

        private static void PrintSeparator()
        {
            Console.WriteLine(
                "------------------------------------------------------------");
        }

        private static void RemoveNtSymbols(
            SortedDictionary<string, SortedSet<string>> Categories)
        {
            string[] Modules = { "ntdll.dll", "ntoskrnl.exe" };
            foreach (string Module in Modules)
            {
                if (!Categories.ContainsKey(Module))
                {
                    continue;
                }
                SortedSet<string> Modified = new SortedSet<string>();
                foreach (string Symbol in Categories[Module])
                {
                    if (char.IsUpper(Symbol[0]) ||
                        Symbol.StartsWith("psMUITest") ||
                        Symbol.StartsWith("vDbg") ||
                        Symbol.StartsWith("@Rtl"))
                    {
                        continue;
                    }
                    Modified.Add(Symbol);
                }
                Categories[Module] = Modified;
            }
        }

        private static void PrintNtCrtSymbolsLists(
            SortedDictionary<string, SortedSet<string>> Categories)
        {
            RemoveNtSymbols(Categories);
            foreach (var Category in Categories)
            {
                Console.WriteLine("{0}", Category.Key);
                foreach (string Symbol in Category.Value)
                {
                    Console.WriteLine("\t{0}", Symbol);
                }
            }
        }

        private static void GenerateNtDllCrtSymbolsLists()
        {
            SortedDictionary<string, List<(string, string)>> Sources =
                new SortedDictionary<string, List<(string, string)>>();
            foreach (string Platform in SupportedPlatforms)
            {
                ImageArchive.Archive Library =
                    ImageArchive.Parse(
                        string.Format(
                            @"{0}\Lib\10.0.26100.0\{1}\ntdllp.lib",
                            ReferencesRootPath,
                            Platform));
                if (Library.Symbols != null)
                {
                    Sources.Add(Platform, Library.Symbols);
                }
                if (Library.EcSymbols != null)
                {
                    Sources.Add("arm64ec", Library.EcSymbols);
                }
            }
            Console.WriteLine("ntdllp.lib");
            PrintSeparator();
            foreach (var Source in Sources)
            {
                Console.WriteLine("{0} Symbols", Source.Key);
                PrintSeparator();
                PrintNtCrtSymbolsLists(
                    ImageArchive.CategorizeSymbols(
                        Source.Value,
                        Source.Key == "x86"));
                PrintSeparator();
            }
        }

        private static void GenerateNtKernelCrtSymbolsLists()
        {
            SortedDictionary<string, List<(string, string)>> Sources =
                new SortedDictionary<string, List<(string, string)>>();
            foreach (string Platform in SupportedPlatforms)
            {
                if (Platform == "x86")
                {
                    continue;
                }
                ImageArchive.Archive Library =
                    ImageArchive.Parse(
                        string.Format(
                            @"{0}\Lib\10.0.26100.0\{1}\ntoskrnl.lib",
                            ReferencesRootPath,
                            Platform));
                if (Library.Symbols != null)
                {
                    Sources.Add(Platform, Library.Symbols);
                }
            }
            Console.WriteLine("ntoskrnl.lib");
            PrintSeparator();
            foreach (var Source in Sources)
            {
                Console.WriteLine("{0} Symbols", Source.Key);
                PrintSeparator();
                PrintNtCrtSymbolsLists(
                    ImageArchive.CategorizeSymbols(Source.Value));
                PrintSeparator();
            }
        }

        static void Main(string[] args)
        {
            GenerateNtDllCrtSymbolsLists();
            GenerateNtKernelCrtSymbolsLists();

            Console.WriteLine(
                "{0} task has been completed.",
                "Mile.NtCrt.UserModeCrtLibraryGenerator");
            Console.ReadKey();
        }
    }
}
