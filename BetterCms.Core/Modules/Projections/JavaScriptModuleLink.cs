// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JavaScriptModuleLink.cs" company="Devbridge Group LLC">
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
using System.Web.Mvc;

using Common.Logging;

using BetterModules.Core.Web.Modules;

namespace BetterCms.Core.Modules.Projections
{
    public class JavaScriptModuleLink : IActionProjection
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private string linkName;

        private JsIncludeDescriptor descriptor;

        private string path;

        /// <summary>
        /// Initializes a new instance of the <see cref="JavaScriptModuleLink" /> class.
        /// </summary>
        /// <param name="descriptor">The js module include.</param>
        /// <param name="linkName">Name of the link.</param>
        /// <param name="path">The path.</param>
        public JavaScriptModuleLink(JsIncludeDescriptor descriptor, string linkName, string path)
        {
            this.path = path;
            this.descriptor = descriptor;
            this.linkName = linkName;
        }

        /// <summary>
        /// Gets or sets the order.
        /// </summary>
        /// <value>
        /// The order.
        /// </value>
        public int Order { get; set; }

        public void Render(HtmlHelper html)
        {
            try
            {
                string link = string.Format("{0}.links.{1} = '{2}';", descriptor.FriendlyName, linkName, path);
                html.ViewContext.Writer.WriteLine(link);
            }
            catch (Exception ex)
            {
                Log.WarnFormat("Failed to render link '{0}' from expression {1} for java script module {2}.", ex, linkName, path, descriptor);
            }
        }
    }
}
