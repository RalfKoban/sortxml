using System.IO;
using System.Text;

namespace MiKoSolutions.SortXml.Sorters
{
    internal sealed class Utf8StringWriter : StringWriter
    {
        public override Encoding Encoding => Encoding.UTF8;
    }
}