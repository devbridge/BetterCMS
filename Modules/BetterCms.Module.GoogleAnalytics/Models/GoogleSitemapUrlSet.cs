// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GoogleSitemapUrlSet.cs" company="Devbridge Group LLC">
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