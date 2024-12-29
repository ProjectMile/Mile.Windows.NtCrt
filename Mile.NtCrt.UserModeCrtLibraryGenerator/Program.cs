using Mile.Project.Helpers;

namespace Mile.NtCrt.UserModeCrtLibraryGenerator
{
    internal class Program
    {
        private static readonly string ProjectRootPath =
           GitRepository.GetRootPath();

        private static readonly string ReferencesRootPath =
            ProjectRootPath + @"\Mile.NtCrt.References";

        private static readonly string ConsoleOutputSeparator =
            "------------------------------------------------------------";

        private static readonly string[] SupportedPlatforms =
        {
            "arm64",
            "x64",
            "x86"
        };

        private static readonly string[] ExcludedNtPrefixes =
        {
            "A_",
            "Alpc",
            "Cpu",
            "Csr",
            "Dbg",
            "Etw",
            "Evt",
            "Ki",
            "Ldr",
            "MD4",
            "MD5",
            "Microsoft",
            "Nls",
            "Nt",
            "Pfx",
            "Process",
            "Pss",
            "Rtl",
            "Sb",
            "Ship",
            "Tp",
            "vDbg",
            "Ver",
            "Wer",
            "Win",
            "Zw"
        };

        private static void PrintSeparator()
        {
            Console.WriteLine(ConsoleOutputSeparator);
        }

        private static void PrintNtDllCrtSymbolsLists(
            SortedDictionary<string, SortedSet<string>> Categories)
        {
            Console.WriteLine("ntdll.dll");
            foreach (string Symbol in Categories["ntdll.dll"])
            {
                bool Excluded = false;
                foreach (string ExcludedPrefix in ExcludedNtPrefixes)
                {
                    if (Symbol.StartsWith(ExcludedPrefix))
                    {
                        Excluded = true;
                        break;
                    }
                }
                if (Excluded)
                {
                    continue;
                }
                Console.WriteLine("\t{0}", Symbol);
            }
            Categories.Remove("ntdll.dll");
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
            foreach (string Platform in SupportedPlatforms)
            {
                ImageArchive.Archive Library =
                    ImageArchive.Parse(
                        string.Format(
                            @"{0}\Lib\10.0.26100.0\{1}\ntdllp.lib",
                            ReferencesRootPath,
                            Platform));
                Console.WriteLine("{0} Symbols", Platform);
                PrintSeparator();
                PrintNtDllCrtSymbolsLists(
                    ImageArchive.CategorizeSymbols(
                        Library.Symbols,
                        Platform == "x86"));
                PrintSeparator();
                if (Library.EcSymbols == null)
                {
                    continue;
                }
                Console.WriteLine("arm64ec Symbols");
                PrintSeparator();
                PrintNtDllCrtSymbolsLists(
                    ImageArchive.CategorizeSymbols(Library.EcSymbols));
                PrintSeparator();
            }
        }

        static void Main(string[] args)
        {
            GenerateNtDllCrtSymbolsLists();

            Console.WriteLine(
                "{0} task has been completed.",
                "Mile.NtCrt.UserModeCrtLibraryGenerator");
            Console.ReadKey();
        }
    }
}
