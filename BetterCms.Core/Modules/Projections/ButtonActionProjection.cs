using System;

using BetterCms.Core.DataContracts;
using BetterCms.Core.Models;

namespace BetterCms.Core.Modules.Projections
{
    /// <summary>
    /// Defines button rendering logic with attached properties to call an action in the JavaScript module.
    /// </summary>
    public class ButtonActionProjection : ActionCallProjectionBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ButtonActionProjection" /> class.
        /// </summary>
        /// <param name="parentModule">The parent module.</param>
        /// <param name="onClickAction">The on click action.</param>
        public ButtonActionProjection(JavaScriptModuleDescriptor parentModule, Func<IPage, string> onClickAction)
            : base("div", parentModule, onClickAction)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ButtonActionProjection" /> class.
        /// </summary>
        /// <param name="parentModule">The parent module.</param>
        /// <param name="title">Button title.</param>
        /// <param name="onClickAction">Name of the action to execute after button click.</param>
        public ButtonActionProjection(JavaScriptModuleDescriptor parentModule, Func<string> title, Func<IPage, string> onClickAction)
            : base("div", parentModule, title, onClickAction)
        {
        }
    }
}
