using System;
using System.IO;

using MiKoSolutions.SortXml.Sorters;

namespace MiKoSolutions.SortXml
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                return;
            }

            var inputFileName = args[0];
            var outputFileName = args[args.Length -1];

            var fileInfo = new FileInfo(inputFileName);
            var fileExtension = fileInfo.Extension;

            if (fileExtension.IndexOf("proj", StringComparison.OrdinalIgnoreCase) != -1)
            {
                ProjectSorter.Sort(inputFileName, outputFileName);
                return;
            }

            if (fileInfo.Name.Equals("packages.config", StringComparison.OrdinalIgnoreCase))
            {
                PackagesConfigSorter.Sort(inputFileName, outputFileName);
                return;
            }

            AttributesSorter.Sort(inputFileName, outputFileName);
        }
    }
}
