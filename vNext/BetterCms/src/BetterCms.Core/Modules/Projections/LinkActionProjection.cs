using System;

using BetterCms.Core.DataContracts;

using BetterModules.Core.Web.Modules;

namespace BetterCms.Core.Modules.Projections
{
    /// <summary>
    /// Defines link rendering logic with attached properties to call an action in the JavaScript module.
    /// </summary>
    public class LinkActionProjection : ActionCallProjectionBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LinkActionProjection" /> class.
        /// </summary>
        /// <param name="parentModuleInclude">The parent module.</param>
        /// <param name="onClickAction">Name of the action to execute after button click.</param>
        public LinkActionProjection(JsIncludeDescriptor parentModuleInclude, Func<IPage, string> onClickAction)
            : base("a", parentModuleInclude, onClickAction)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LinkActionProjection" /> class.
        /// </summary>
        /// <param name="parentModuleInclude">The parent module.</param>
        /// <param name="title">Link title.</param>
        /// <param name="onClickAction">Name of the action to execute after button click.</param>
        public LinkActionProjection(JsIncludeDescriptor parentModuleInclude, Func<IPage, string> title, Func<IPage, string> onClickAction)
            : base("a", parentModuleInclude, title, onClickAction)
        {
        }

        /// <summary>
        /// Called before render methods sends element to response output.
        /// </summary>
        /// <param name="controlRenderer">The html control renderer.</param>
        /// <param name="page">The page.</param>
        /// <param name="html">The html helper.</param>
        protected override void OnPreRender(HtmlControlRenderer controlRenderer, IPage page, System.Web.Mvc.HtmlHelper html)
        {
            base.OnPreRender(controlRenderer, page, html);

            controlRenderer.Attributes.Add("href", "#");
        }
    }
}
