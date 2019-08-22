using System.Linq;
using System.Xml.Linq;

namespace MiKoSolutions.SortXml.Sorters
{
    internal static class AttributesSorter
    {
        internal static void Sort(string inputFileName, string outputFileName)
        {
            var xDocument = XDocument.Load(inputFileName, LoadOptions.PreserveWhitespace | LoadOptions.SetLineInfo);
            var xElements = xDocument.Descendants();
            foreach (var element in xElements)
            {
                var attributes = element.Attributes().OrderBy(_ => _.Name.LocalName).ToArray<object>();
                element.RemoveAttributes();
                element.Add(attributes);
            }

            xDocument.Save(outputFileName, SaveOptions.DisableFormatting);
        }
    }
}