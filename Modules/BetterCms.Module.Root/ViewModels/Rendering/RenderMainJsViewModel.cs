// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RenderMainJsViewModel.cs" company="Devbridge Group LLC">
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

namespace BetterCms.Module.Root.ViewModels.Rendering
{
    /// <summary>
    /// View model for dynamic java script file (main.js) initialization.
    /// </summary>
    public class RenderMainJsViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RenderMainJsViewModel" /> class.
        /// </summary>
        public RenderMainJsViewModel()
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
        /// Gets or sets a value indicating whether debug mode is on.
        /// </summary>
        /// <value>
        /// <c>true</c> if debug mode is on; otherwise, <c>false</c>.
        /// </value>
        public bool IsDebugMode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use *.min.js references.
        /// </summary>
        /// <value>
        ///   <c>true</c> if use *.min.js references; otherwise, <c>false</c>.
        /// </value>
        public bool UseMinReferences { get; set; }

        /// <summary>
        /// Gets or sets the CMS version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public string Version { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("IsDebugMode: {0}", IsDebugMode);
        }
    }
}