// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GoogleSitemapUrl.cs" company="Devbridge Group LLC">
// 
// Copyright (C) 2015,2016 Devbridge Group LLC
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with this program.  If not, see http://www.gnu.org/licenses/. 
// </copyright>
// 
// <summary>
// Better CMS is a publishing focused and developer friendly .NET open source CMS.
// 
// Website: https://www.bettercms.com 
// GitHub: https://github.com/devbridge/bettercms
// Email: info@bettercms.com
// </summary>
// --------------------------------------------------------------------------------------------------------------------
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