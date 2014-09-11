using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BetterCms.Module.GoogleAnalytics.Models
{
    public class GoogleSitemapUrl
    {
        private readonly string dateTimeFormat;

        public GoogleSitemapUrl()
        {
            
        }

        public GoogleSitemapUrl(string dateTimeformat)
        {
            Links = new List<GoogleSitemapLink>();
            dateTimeFormat = dateTimeformat;
        }

        [XmlElement(ElementName = "loc")]
        public string Location { get; set; }

        [XmlElement("link", Namespace = "http://www.w3.org/1999/xhtml")]
        public List<GoogleSitemapLink> Links { get; set; }

        [XmlIgnore]
        public DateTime LastModifiedDateTime { get; set; }

        [XmlElement(ElementName = "lastmod")]
        public string LastModifiedString
        {
            get { return LastModifiedDateTime.ToString(dateTimeFormat); }
            set { LastModifiedDateTime = DateTime.Parse(value); }
        }

        [XmlElement(ElementName = "changefreq")]
        public string ChangeFrequency { get; set; }

        [XmlElement(ElementName = "priority")]
        public string Priority { get; set; }
    }
}