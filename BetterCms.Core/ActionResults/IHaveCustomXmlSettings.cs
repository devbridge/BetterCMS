using System.Text;
using System.Xml.Serialization;

namespace BetterCms.Core.ActionResults
{
    public interface IHaveCustomXmlSettings
    {
        XmlSerializerNamespaces Namespaces { get; }

        Encoding GetEncoding();
    }
}
