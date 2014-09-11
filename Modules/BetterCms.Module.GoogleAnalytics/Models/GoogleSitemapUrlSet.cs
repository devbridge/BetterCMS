using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

using BetterCms.Core.ActionResults;

namespace BetterCms.Module.GoogleAnalytics.Models
{
    [XmlRoot("urlset", Namespace = "http://www.sitemaps.org/schemas/sitemap/0.9", IsNullable = false)]
    public class GoogleSitemapUrlSet : IHaveCustomXmlSettings
    {
        public GoogleSitemapUrlSet()
        {
            Urls = new List<GoogleSitemapUrl>();

            _namespaces =
                new XmlSerializerNamespaces(
                    new[]
                    { new XmlQualifiedName("xhtml", "http://www.w3.org/1999/xhtml"), 
                        new XmlQualifiedName(string.Empty, "http://www.sitemaps.org/schemas/sitemap/0.9") });
        }

        [XmlElement(ElementName = "url")]
        public List<GoogleSitemapUrl> Urls { get; set; }

        public Encoding GetEncoding()
        {
            return Encoding.UTF8;
        }

        [XmlNamespaceDeclarations]
        public XmlSerializerNamespaces Namespaces
        {
            get
            {
                return _namespaces;
            }
        }

        private XmlSerializerNamespaces _namespaces;
    }
}