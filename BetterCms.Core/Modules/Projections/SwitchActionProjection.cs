// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SwitchActionProjection.cs" company="Devbridge Group LLC">
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
using System.Web.UI.HtmlControls;

using BetterCms.Core.DataContracts;

namespace BetterCms.Core.Modules.Projections
{
    /// <summary>
    /// Defines button rendering logic with attached properties to call an action in the JavaScript module.
    /// </summary>
    public class SwitchActionProjection : HtmlElementProjection
    {
        /// <summary>
        /// Module action marker css class.
        /// </summary>
        private const string ModuleActionMarkerCssClass = "bcms-onclick-action";

        /// <summary>
        /// Html element attribute name to mark module name.
        /// </summary>
        private const string ModuleNameAttribute = "data-bcms-module";

        /// <summary>
        /// Html element attribute name to mark module action name.
        /// </summary>
        private const string ModuleActionAttribute = "data-bcms-action";
        /// <summary>
        /// A client side parent module.
        /// </summary>
        private readonly JsIncludeDescriptor parentModuleInclude;

        /// <summary>
        /// Initializes a new instance of the <see cref="ButtonActionProjection" /> class.
        /// </summary>
        /// <param name="parentModuleInclude">The parent module.</param>
        /// <param name="onClickAction">The on click action.</param>
        public SwitchActionProjection(JsIncludeDescriptor parentModuleInclude, Func<IPage, string> onClickAction)
            : base("div")
        {
            this.parentModuleInclude = parentModuleInclude;
            OnClickAction = onClickAction;
        }

        /// <summary>
        /// Gets or sets the callback function attached to action.
        /// </summary>
        /// <value>
        /// The callback function.
        /// </value>
        public Func<IPage, string> OnClickAction { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public Func<IPage, string> Title { get; set; }

        protected override void OnPreRender(HtmlControlRenderer controlRenderer, IPage page, HtmlHelper html)
        {
            base.OnPreRender(controlRenderer, page, html);

            if (Title != null)
            {
                var title = Title(page);
                controlRenderer.Attributes["title"] = title;
            }

            var innerDivOn = new HtmlGenericControl("div");
            innerDivOn.Attributes["class"] = "bcms-switch-text";
            innerDivOn.Controls.Add(new LiteralControl("Yes"));
            controlRenderer.Controls.Add(innerDivOn);

            var innerDivOff = new HtmlGenericControl("div");
            innerDivOff.Attributes["class"] = "bcms-switch-text";
            innerDivOff.Controls.Add(new LiteralControl("No"));
            controlRenderer.Controls.Add(innerDivOff);

            if (OnClickAction == null || parentModuleInclude == null)
            {
                return;
            }

            var cssClass = controlRenderer.Attributes["class"];
            if (!string.IsNullOrEmpty(cssClass) && !cssClass.Contains(ModuleActionMarkerCssClass))
            {
                cssClass = cssClass + " " + ModuleActionMarkerCssClass;
            }
            else
            {
                cssClass = ModuleActionMarkerCssClass;
            }

            controlRenderer.Attributes["class"] = cssClass;
            controlRenderer.Attributes.Add(ModuleNameAttribute, parentModuleInclude.Name);
            controlRenderer.Attributes.Add(ModuleActionAttribute, OnClickAction(page));
        }
    }
}
