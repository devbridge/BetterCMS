using System.Xml.Serialization;

namespace BetterCms.Module.GoogleAnalytics.Models
{
    public class GoogleSitemapLink
    {
        [XmlAttribute("rel")]
        public string LinkType { get; set; }

        [XmlAttribute("hreflang")]
        public string LanguageCode { get; set; }

        [XmlAttribute("href")]
        public string Url { get; set; }
    }
}