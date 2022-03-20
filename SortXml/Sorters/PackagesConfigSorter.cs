using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace MiKoSolutions.SortXml.Sorters
{
    internal static class PackagesConfigSorter
    {
        internal static void Sort(string inputFileName, string outputFileName)
        {
            var xDocument = XDocument.Load(inputFileName, LoadOptions.PreserveWhitespace | LoadOptions.SetLineInfo);

            var packages = xDocument.Descendants().Where(_ => _.Name.LocalName == "packages");
            foreach (var package in packages)
            {
                var descendants = package.Descendants("package").OrderBy(_ => _.Attribute("id").Value).ToList();
                package.RemoveAll();
                package.Add(descendants);
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
    }
}