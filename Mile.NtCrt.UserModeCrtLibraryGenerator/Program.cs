﻿using Mile.NtCrt.GeneratorCore;
using Mile.Project.Helpers;

namespace Mile.NtCrt.UserModeCrtLibraryGenerator
{
    internal class Program
    {
        private static readonly string[] SupportedPlatforms =
        {
            "arm64",
            "x64",
            "x86"
        };

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
                        ProjectPath.GetLibraryPath(
                            "10.0.26100.0",
                            Platform,
                            "ntdllp.lib"));
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
            Utilities.PrintSeparator();
            foreach (var Source in Sources)
            {
                Console.WriteLine("{0} Symbols", Source.Key);
                Utilities.PrintSeparator();
                PrintNtCrtSymbolsLists(
                    ImageArchive.CategorizeSymbols(
                        Source.Value,
                        Source.Key == "x86"));
                Utilities.PrintSeparator();
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
                        ProjectPath.GetLibraryPath(
                            "10.0.26100.0",
                            Platform,
                            "ntoskrnl.lib"));
                if (Library.Symbols != null)
                {
                    Sources.Add(Platform, Library.Symbols);
                }
            }
            Console.WriteLine("ntoskrnl.lib");
            Utilities.PrintSeparator();
            foreach (var Source in Sources)
            {
                Console.WriteLine("{0} Symbols", Source.Key);
                Utilities.PrintSeparator();
                PrintNtCrtSymbolsLists(
                    ImageArchive.CategorizeSymbols(Source.Value));
                Utilities.PrintSeparator();
            }
        }

        private static void GenerateAdditionalSymbolLists()
        {
            Console.WriteLine("Additional Symbols");
            Utilities.PrintSeparator();
            foreach (string Platform in SupportedPlatforms)
            {
                if (Platform == "x86")
                {
                    continue;
                }
                Console.WriteLine("{0} Symbols", Platform);
                Utilities.PrintSeparator();
                SortedSet<string> StaticList = ImageArchive.ListSymbols(
                    ImageArchive.Parse(
                        ProjectPath.GetLibraryPath(
                            "10.0.26100.0",
                            Platform,
                            "libcntpr.lib")).Symbols);
                SortedDictionary<string, SortedSet<string>> DynamicCategories =
                    ImageArchive.CategorizeSymbols(
                        ImageArchive.Parse(
                            ProjectPath.GetLibraryPath(
                                "10.0.26100.0",
                                Platform,
                                "ntoskrnl.lib")).Symbols);
                RemoveNtSymbols(DynamicCategories);
                SortedSet<string> DynamicList =
                    ImageArchive.ListSymbols(DynamicCategories);
                foreach (string Symbol in DynamicList)
                {
                    if (!StaticList.Contains(Symbol))
                    {
                        Console.WriteLine(Symbol);
                    }
                }
                Utilities.PrintSeparator();
            }
        }

        static void Main(string[] args)
        {
            GenerateNtDllCrtSymbolsLists();
            GenerateNtKernelCrtSymbolsLists();
            GenerateAdditionalSymbolLists();

            Console.WriteLine(
                "{0} task has been completed.",
                "Mile.NtCrt.UserModeCrtLibraryGenerator");
            Console.ReadKey();
        }
    }
}
