// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InstallationModuleDescriptor.cs" company="Devbridge Group LLC">
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

using BetterCms.Core.Modules;

namespace BetterCms.Module.Installation
{
    /// <summary>
    /// Templates module descriptor.
    /// </summary>
    public class InstallationModuleDescriptor : CmsModuleDescriptor
    {
        internal const string ModuleName = "installation";

        internal const string ModuleAreaName = "bcms-installation";

        internal const string ModuleSchemaName = "bcms_installation";

        /// <summary>
        /// Initializes a new instance of the <see cref="InstallationModuleDescriptor" /> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public InstallationModuleDescriptor(ICmsConfiguration configuration) : base(configuration)
        {            
        }

        /// <summary>
        /// Gets the name of module.
        /// </summary>
        /// <value>
        /// The name of pages module.
        /// </value>
        public override string Name
        {
            get
            {
                return ModuleName;
            }
        }

        /// <summary>
        /// Gets the name of the module database schema name.
        /// </summary>
        /// <value>
        /// The name of the module database schema.
        /// </value>
        public override string SchemaName
        {
            get
            {
                return ModuleSchemaName;
            }
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>
        /// The module description.
        /// </value>
        public override string Description
        {
            get
            {
                return "Templates module for Better CMS.";
            }
        }

        /// <summary>
        /// Gets the order.
        /// </summary>
        /// <value>
        /// The order.
        /// </value>
        public override int Order
        {
            get
            {
                return int.MaxValue - 100;
            }
        }

        /// <summary>
        /// Registers the style sheet files.
        /// </summary>
        /// <returns>
        /// Enumerator of known module style sheet files.
        /// </returns>
        public override IEnumerable<CssIncludeDescriptor> RegisterCssIncludes()
        {
            return new[] { new CssIncludeDescriptor(this, "bcms.installation.css", "bcms.installation.min.css", true) };
        }
    }
}
