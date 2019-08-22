using MiKoSolutions.SortXml.Sorters;

namespace MiKoSolutions.SortXml
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
                return;

            var inputFileName = args[0];
            var outputFileName = args[args.Length -1];

            if (inputFileName.EndsWith(".csproj"))
            {
                CSharpProjectSorter.Sort(inputFileName, outputFileName);
            }
            else
            {
                AttributesSorter.Sort(inputFileName, outputFileName);
            }
        }
    }
}
