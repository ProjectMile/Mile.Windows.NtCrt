using Mile.Project.Helpers;

namespace Mile.NtCrt.UefiCrtLibraryGenerator
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
            "x64"
        };

        private static void GenerateUefiCrtObjectsSymbolsLists()
        {
            SortedDictionary<string, int> ReferenceCounts =
               new SortedDictionary<string, int>();
            foreach (string Platform in SupportedPlatforms)
            {
                ImageArchive.Archive Library =
                    ImageArchive.Parse(
                        string.Format(
                            @"{0}\Lib\10.0.26100.0\{1}\libcntpr.lib",
                            ReferencesRootPath,
                            Platform));
                SortedSet<string> Symbols =
                    ImageArchive.ListSymbols(Library.Symbols);
                foreach (string Symbol in Symbols)
                {
                    if (ReferenceCounts.ContainsKey(Symbol))
                    {
                        ReferenceCounts[Symbol]++;
                    }
                    else
                    {
                        ReferenceCounts.Add(Symbol, 1);
                    }
                }
            }

            SortedDictionary<string, string> References =
                new SortedDictionary<string, string>();
            foreach (string Platform in SupportedPlatforms)
            {
                ImageArchive.Archive Library =
                    ImageArchive.Parse(
                        string.Format(
                            @"{0}\Lib\10.0.26100.0\{1}\libcntpr.lib",
                            ReferencesRootPath,
                            Platform));
                SortedDictionary<string, SortedSet<string>> Categories =
                    ImageArchive.CategorizeSymbols(Library.Symbols);
                foreach (var Category in Categories)
                {
                    foreach (string Symbol in Category.Value)
                    {
                        string SymbolKey = string.Format(
                            "{0}!{1}",
                            Path.GetFileName(Category.Key),
                            Symbol);
                        if (!References.ContainsKey(SymbolKey))
                        {
                            References.Add(SymbolKey, Category.Key);
                        }
                        else
                        {
                            References[SymbolKey] += ";" + Category.Key;
                        }
                    }
                }
            }

            SortedSet<string> IncludeObjectList = new SortedSet<string>();
            foreach (var Reference in References)
            {
                string SymbolName = Reference.Key.Split('!')[1];
                if (ReferenceCounts[SymbolName] == SupportedPlatforms.Length)
                {
                    foreach (string Object in Reference.Value.Split(';'))
                    {
                        IncludeObjectList.Add(Object);
                    }
                }
            }

            foreach (string Platform in SupportedPlatforms)
            {
                ImageArchive.Archive Library =
                    ImageArchive.Parse(
                        string.Format(
                            @"{0}\Lib\10.0.26100.0\{1}\libcntpr.lib",
                            ReferencesRootPath,
                            Platform));
                SortedDictionary<string, SortedSet<string>> Categories =
                    ImageArchive.CategorizeSymbols(Library.Symbols);
                Console.WriteLine("{0} Common Symbols", Platform);
                Console.WriteLine("----------------------------------------");
                foreach (var Category in Categories)
                {
                    if (IncludeObjectList.Contains(Category.Key))
                    {
                        Console.WriteLine("{0}", Category.Key);
                        foreach (string Symbol in Category.Value)
                        {
                            int ReferenceCount = ReferenceCounts[Symbol];
                            if (ReferenceCount == SupportedPlatforms.Length)
                            {
                                Console.WriteLine("\t{0}", Symbol);
                            }
                        }
                    }
                }
                Console.WriteLine("----------------------------------------");
            }

            foreach (string Platform in SupportedPlatforms)
            {
                ImageArchive.Archive Library =
                    ImageArchive.Parse(
                        string.Format(
                            @"{0}\Lib\10.0.26100.0\{1}\libcntpr.lib",
                            ReferencesRootPath,
                            Platform));
                SortedDictionary<string, SortedSet<string>> Categories =
                    ImageArchive.CategorizeSymbols(Library.Symbols);
                Console.WriteLine("{0} Specialized Symbols", Platform);
                Console.WriteLine("----------------------------------------");
                foreach (var Category in Categories)
                {
                    if (!IncludeObjectList.Contains(Category.Key))
                    {
                        Console.WriteLine("{0}", Category.Key);
                    }
                }
                Console.WriteLine("----------------------------------------");
            }
        }

        static void Main(string[] args)
        {
            GenerateUefiCrtObjectsSymbolsLists();

            Console.WriteLine(
                "{0} task has been completed.",
                "Mile.NtCrt.UefiCrtLibraryGenerator");
            Console.ReadKey();
        }
    }
}
