using System;
using System.Security.Principal;
using System.Web.Mvc;

using BetterCms.Core.DataContracts;
using BetterCms.Core.Models;
using BetterCms.Core.Services;

namespace BetterCms.Core.Modules.Projections
{
    public interface IActionProjection
    {
        /// <summary>
        /// Gets or sets the order.
        /// </summary>
        /// <value>
        /// The order.
        /// </value>
        int Order { get; }

        /// <summary>
        /// Renders an action projection to given html output.
        /// </summary>
        /// <param name="html">The html helper.</param>
        void Render(HtmlHelper html);
    }

    /// <summary>
    /// Defines the contract for action projection rendering.
    /// </summary>
    public interface IPageActionProjection
    {        
        /// <summary>
        /// Gets or sets the order.
        /// </summary>
        /// <value>
        /// The order.
        /// </value>
        int Order { get; }

        /// <summary>
        /// Gets or sets permission for rendering.
        /// </summary>
        /// <value>
        /// The role.
        /// </value>
        string AccessRole { get; set; }

        /// <summary>
        /// Renders an action projection to given html output.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="securityService"></param>
        /// <param name="html">The html helper.</param>
        void Render(IPage page, ISecurityService securityService, HtmlHelper html);
    }
}
