using System;
using System.Diagnostics.CodeAnalysis;
using System.Web.Mvc;
using System.Web.UI;

using BetterCms.Core.DataContracts;

namespace BetterCms.Core.Modules.Projections
{
    /// <summary>
    /// Defines generic html element rendering logic with attached properties to call an action in the JavaScript module.
    /// </summary>
    public abstract class ActionCallProjectionBase : HtmlElementProjection
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
        /// Initializes a new instance of the <see cref="ActionCallProjectionBase" /> class.
        /// </summary>
        /// <param name="htmlTag">The HTML tag.</param>
        /// <param name="parentModuleInclude">The parent module.</param>
        /// <param name="onClickAction">Name of the action to execute after button click.</param>
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed. Suppression is OK here.")]
        protected ActionCallProjectionBase(string htmlTag, JsIncludeDescriptor parentModuleInclude, Func<IPage, string> onClickAction)
            : base(htmlTag)
        {
            this.parentModuleInclude = parentModuleInclude;
            OnClickAction = onClickAction;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionCallProjectionBase" /> class.
        /// </summary>
        /// <param name="htmlTag">The HTML tag.</param>
        /// <param name="parentModuleInclude">The parent module (should contain on click action).</param>
        /// <param name="title">The button title.</param>
        /// <param name="onClickAction">Name of the action to execute after button click.</param>
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed. Suppression is OK here.")]
        protected ActionCallProjectionBase(string htmlTag, JsIncludeDescriptor parentModuleInclude, Func<IPage, string> title, Func<IPage, string> onClickAction)
            : base(htmlTag)
        {
            this.parentModuleInclude = parentModuleInclude;
            Title = title;
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

        /// <summary>
        /// Called before render methods sends element to response output.
        /// </summary>
        /// <param name="controlRenderer">The html control renderer.</param>
        /// <param name="page">The page.</param>
        /// <param name="html">The html helper.</param>
        protected override void OnPreRender(HtmlControlRenderer controlRenderer, IPage page, HtmlHelper html)
        {
            base.OnPreRender(controlRenderer, page, html);

            if (Title != null)
            {
                string title = Title(page);
                controlRenderer.Controls.Add(new LiteralControl(title));
            }

            if (OnClickAction != null && parentModuleInclude != null)
            {
                string cssClass = controlRenderer.Attributes["class"];
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
}
