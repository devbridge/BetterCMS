// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RenderProcessorJsViewModel.cs" company="Devbridge Group LLC">
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
using System.Linq;

namespace BetterCms.Module.Root.ViewModels.Rendering
{
    /// <summary>
    /// View model for dynamic java script file (main.js) initialization.
    /// </summary>
    public class RenderProcessorJsViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RenderMainJsViewModel" /> class.
        /// </summary>
        public RenderProcessorJsViewModel()
        {
            JavaScriptModules = new List<JavaScriptModuleInclude>();
        }

        /// <summary>
        /// Gets or sets a list of JS modules.
        /// </summary>
        /// <value>
        /// A list of JS modules.
        /// </value>
        public IEnumerable<JavaScriptModuleInclude> JavaScriptModules { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("JavaScriptModules: {0}", string.Join(", ", JavaScriptModules != null ? JavaScriptModules.Select(f => f.ToString()) : Enumerable.Empty<string>()));
        }
    }
}