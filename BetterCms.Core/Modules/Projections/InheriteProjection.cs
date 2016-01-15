// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InheriteProjection.cs" company="Devbridge Group LLC">
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
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Web.Mvc;
using System.Web.UI;

using BetterCms.Core.DataContracts;
using BetterCms.Core.Services;

namespace BetterCms.Core.Modules.Projections
{
    /// <summary>
    /// Projection to support action projections inheritance.
    /// </summary>
    public class InheriteProjection : HtmlElementProjection
    {        
        /// <summary>
        /// Initializes a new instance of the <see cref="InheriteProjection" /> class.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <param name="childProjections">The child projections.</param>
        public InheriteProjection(string tag, IEnumerable<IPageActionProjection> childProjections)
            : base(tag)
        {
            ChildProjections = childProjections;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InheriteProjection" /> class.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <param name="childProjections">The child projections.</param>
        /// <param name="order">The order.</param>
        public InheriteProjection(string tag, IEnumerable<IPageActionProjection> childProjections, int order)
            : base(tag)
        {
            ChildProjections = childProjections;
            Order = order;
        }

        /// <summary>
        /// Gets or sets the child projections.
        /// </summary>
        /// <value>
        /// The child projections.
        /// </value>
        public IEnumerable<IPageActionProjection> ChildProjections { get; set; }

        /// <summary>
        /// Renders an action projection to given html output.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="securityService"></param>
        /// <param name="html">The html helper.</param>
        /// <returns><c>true</c> on success, otherwise <c>false</c>.</returns>
        public override bool Render(IPage page, ISecurityService securityService, HtmlHelper html)
        {
            if (ShouldBeRendered != null && !ShouldBeRendered(page) || AccessRole != null && !securityService.IsAuthorized(AccessRole))
            {
                return false;
            }

            using (HtmlControlRenderer control = new HtmlControlRenderer(Tag))
            {
                OnPreRender(control, page, html);

                using (HtmlTextWriter writer = new HtmlTextWriter(html.ViewContext.Writer))
                {
                    control.RenderBeginTag(writer);
                    
                    if (ChildProjections != null)
                    {
                        foreach (var htmlElementProjection in ChildProjections.OrderBy(f => f.Order))
                        {
                            htmlElementProjection.Render(page, securityService, html);
                        }
                    }

                    control.RenderEndTag(writer);
                }
            }

            return true;
        }
    }
}
