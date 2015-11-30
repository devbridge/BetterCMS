// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JavaScriptModuleGlobalization.cs" company="Devbridge Group LLC">
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
    /// <summary>
    /// Java script module resource initialization renderer.
    /// </summary>
    public class JavaScriptModuleGlobalization : IActionProjection
    {
        /// <summary>
        /// Current class logger.
        /// </summary>
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Resource name.
        /// </summary>
        private readonly string name;

        /// <summary>
        /// Function to retrieve resource in current culture.
        /// </summary>
        private readonly Func<string> resource;

        /// <summary>
        /// Resources module.
        /// </summary>
        private readonly JsIncludeDescriptor jsModuleInclude;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="JavaScriptModuleGlobalization" /> class.
        /// </summary>
        /// <param name="jsModuleInclude">The java script module.</param>
        /// <param name="name">The name.</param>
        /// <param name="resource">A function to retrieve resource in current culture.</param>
        public JavaScriptModuleGlobalization(JsIncludeDescriptor jsModuleInclude, string name, Func<string> resource)
        {
            this.jsModuleInclude = jsModuleInclude;
            this.name = name;
            this.resource = resource;
        }

        /// <summary>
        /// Gets or sets the order.
        /// </summary>
        /// <value>
        /// The order.
        /// </value>
        public int Order { get; set; }

        /// <summary>
        /// Renders java script module resource initialization. 
        /// </summary>
        /// <param name="html">Html helper.</param>
        public void Render(HtmlHelper html)
        {
            try
            {
                var resourceObject = resource();
                if (resourceObject != null)
                {
                    string globalization = string.Format("{0}.globalization.{1} = '{2}';", jsModuleInclude.FriendlyName, name, resourceObject.Replace("'", "\\'"));
                    html.ViewContext.Writer.WriteLine(globalization);
                }
                else
                {
                    Log.WarnFormat("Resource object not found to globalize {0}.{1} from resource {2}.", jsModuleInclude, name, resource);
                }
            }
            catch (Exception ex)
            {
                Log.WarnFormat("Failed to render globalization for {0}.{1} from resource {2}.", ex, jsModuleInclude, name, jsModuleInclude);
            }
        }        
    }
}
