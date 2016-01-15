// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RenderWidgetViewModel.cs" company="Devbridge Group LLC">
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

using BetterCms.Core.DataContracts;

namespace BetterCms.Module.Root.ViewModels.Cms
{
    /// <summary>
    /// Render view model for server widgets.
    /// </summary>
    public class RenderWidgetViewModel
    {
        /// <summary>
        /// Gets or sets the page.
        /// </summary>
        /// <value>
        /// The page.
        /// </value>
        public IRenderPage Page { get; set; }

        /// <summary>
        /// Gets or sets the widget.
        /// </summary>
        /// <value>
        /// The widget.
        /// </value>
        public IWidget Widget { get; set; }

        /// <summary>
        /// Gets or sets the widget options.
        /// </summary>
        /// <value>
        /// The widget options.
        /// </value>
        public IList<IOptionValue> Options { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            if (Page != null)
            {
                sb.AppendFormat("Page: Id={0}, Title={1}", Page.Id, Page.Title);
            }
            else
            {
                sb.AppendFormat("Page property is empty");
            }

            sb.Append("; ");

            if (Widget != null)
            {
                sb.AppendFormat("Widget: Id={0}, Name={1}", Widget.Id, Widget.Name);
            }
            else
            {
                sb.AppendFormat("Widget property is empty");
            }

            sb.Append(".");

            return sb.ToString();
        }
    }
}