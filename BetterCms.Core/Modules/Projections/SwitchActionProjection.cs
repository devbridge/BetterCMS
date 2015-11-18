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
