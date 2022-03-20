using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace MiKoSolutions.SortXml.Sorters
{
    internal static class ProjectSorter
    {
        internal static void Sort(string inputFileName, string outputFileName)
        {
            var xDocument = XDocument.Load(inputFileName, LoadOptions.PreserveWhitespace | LoadOptions.SetLineInfo);

            // ensure that we have UTF-8 encoding
            xDocument.Declaration = new XDeclaration("1.0", "utf-8", null);

            var itemGroups = xDocument.Descendants().Where(_ => _.Name.LocalName == "ItemGroup");
            foreach (var itemGroup in itemGroups.Where(_ => _.HasElements))
            {
                var elements = itemGroup.Elements()
                                        .OrderBy(_ => _.Name.LocalName)
                                        .ThenBy(_ => _.FirstAttribute?.Value ??_.Value)
                                        .ToArray();


                // sort all element contents
                foreach (var element in elements.Where(_ => _.Name.LocalName != "ProjectReference"))
                {
                    var nested = element.Elements()
                                        .OrderBy(_ => _.Name.LocalName)
                                        .ThenBy(_ => _.Value)
                                        .ToArray();

                    element.RemoveNodes();
                    element.Add(nested);
                }

                itemGroup.RemoveNodes();
                itemGroup.Add(elements);
            }

            SaveXml(xDocument, outputFileName);
        }

        private static void SaveXml(XDocument document, string outputFileName)
        {
            // save with intendation (hence we need a document)
            var doc = new XmlDocument();

            using (StringWriter writer = new Utf8StringWriter())
            {
                document.Save(writer, SaveOptions.None);
                writer.Flush();

                var xml = writer.GetStringBuilder().ToString();

                doc.LoadXml(xml);
            }

            File.SetAttributes(outputFileName, FileAttributes.Normal);

            doc.Save(outputFileName);
        }

        internal static void FindItemGroupIssues(string inputFileName, string outputFileName)
        {
            var xDocument = XDocument.Load(inputFileName, LoadOptions.PreserveWhitespace | LoadOptions.SetLineInfo);

            // ensure that we have UTF-8 encoding
            xDocument.Declaration = new XDeclaration("1.0", "utf-8", null);

            var issue = false;

            var itemGroups = xDocument.Descendants().Where(_ => _.Name.LocalName == "ItemGroup");
            foreach (var itemGroup in itemGroups.Where(_ => _.HasElements))
            {
                var elements = itemGroup.Elements().ToList();
                var expectedName = elements.First().Name.LocalName;

                foreach (var element in elements)
                {
                    var localName = element.Name.LocalName;
                    if (localName != expectedName)
                    {
                        issue = true;
                    }
                }
            }

            if (issue)
            {
                Trace.WriteLine(inputFileName, "RKN Semantic");
            }
        }
    }
}