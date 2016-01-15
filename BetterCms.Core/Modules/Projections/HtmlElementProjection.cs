// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HtmlElementProjection.cs" company="Devbridge Group LLC">
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
using System.Web.UI;

using BetterCms.Core.DataContracts;
using BetterCms.Core.Services;

namespace BetterCms.Core.Modules.Projections
{
    /// <summary>
    /// Represents base projection to render html elements.
    /// </summary>
    public class HtmlElementProjection : IPageActionProjection
    {
        /// <summary>
        /// Determines, if html tag is self closing
        /// </summary>
        private readonly bool isSelfClosing;

        /// <summary>
        /// Initializes a new instance of the <see cref="HtmlElementProjection" /> class.
        /// </summary>
        /// <param name="tag">The tag key.</param>
        /// <param name="isSelfClosing">if set to <c>true</c> if html tag is self closing.</param>
        public HtmlElementProjection(string tag, bool isSelfClosing = false)
        {
            Tag = tag;

            this.isSelfClosing = isSelfClosing;
        }

        /// <summary>
        /// Gets or sets function to retrieve an id for html element.
        /// </summary>
        /// <value>
        /// A function to retrieve an id for html element.
        /// </value>
        public Func<IPage, string> Id { get; set; }

        /// <summary>
        /// Gets or sets function to retrieve html element CSS class.
        /// </summary>
        /// <value>
        /// A function to retrieve html element CSS class.
        /// </value>
        public Func<IPage, string> CssClass { get; set; }

        /// <summary>
        /// Gets or sets the tooltip.
        /// </summary>
        /// <value>
        /// The tooltip.
        /// </value>
        public Func<IPage, string> Tooltip { get; set; }

        /// <summary>
        /// Determines, if HTML projection should be rendered.
        /// </summary>
        /// <value>
        /// <c>true</c>, if HTML projection should be rendered, otherwise <c>false</c>.
        /// </value>
        public Func<IPage, bool> ShouldBeRendered { get; set; }

        /// <summary>
        /// Gets or sets the order.
        /// </summary>
        /// <value>
        /// The order.
        /// </value>
        public int Order { get; set; }

        /// <summary>
        /// Gets or sets permission for rendering.
        /// </summary>
        /// <value>
        /// The role.
        /// </value>
        public string AccessRole { get; set; }

        /// <summary>
        /// Gets html element tag name.
        /// </summary>
        protected string Tag { get; private set; }

        public Func<IPage, string> InnerHtml { get; set; }

        /// <summary>
        /// Renders a control.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="securityService"></param>
        /// <param name="html">The HTML.</param>
        /// <returns><c>true</c> on success, otherwise <c>false</c>.</returns>
        public virtual bool Render(IPage page, ISecurityService securityService, HtmlHelper html)
        {
            if (ShouldBeRendered != null && !ShouldBeRendered.Invoke(page))
            {
                return false;
            }

            if (AccessRole != null && !securityService.IsAuthorized(AccessRole))
            {
                return false;
            }

            using (HtmlControlRenderer control = new HtmlControlRenderer(Tag, isSelfClosing))
            {
                OnPreRender(control, page, html);

                using (HtmlTextWriter writer = new HtmlTextWriter(html.ViewContext.Writer))
                {
                    control.RenderControl(writer);
                }
            }

            return true;
        }

        /// <summary>
        /// Called before render methods sends element to response output.
        /// </summary>
        /// <param name="controlRenderer">The html control renderer.</param>
        /// <param name="page">The page.</param>
        /// <param name="html">The html helper.</param>
        protected virtual void OnPreRender(HtmlControlRenderer controlRenderer, IPage page, HtmlHelper html)
        {            
            if (Id != null)
            {
                controlRenderer.Attributes["id"] = Id(page);
            }

            if (CssClass != null)
            {
                string css = controlRenderer.Attributes["class"];

                if (!string.IsNullOrEmpty(css))
                {
                    css = string.Concat(css, " ", CssClass(page));
                }
                else
                {
                    css = CssClass(page);
                }

                controlRenderer.Attributes.Add("class", css);
            }

            if (Tooltip != null)
            {
                string tooltip = Tooltip(page);
                controlRenderer.Attributes.Add("title", tooltip);
            }

            controlRenderer.Attributes.Add("data-bcms-order", Order.ToString());

            if (InnerHtml != null)
            {
                controlRenderer.InnerHtml = InnerHtml(page);
            }
        }   
    }
}
