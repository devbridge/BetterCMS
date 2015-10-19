using BetterCms.Core.DataContracts;
using BetterCms.Core.Services;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.AspNet.Mvc.ViewFeatures;

namespace BetterCms.Core.Modules.Projections
{
    public class RenderViewComponentProjection<TViewComponent> : IPageActionProjection where TViewComponent : ViewComponent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RenderViewComponentProjection{TViewComponent}"/> class.
        /// </summary>
        public RenderViewComponentProjection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RenderViewComponentProjection{TViewComponent}"/> class.
        /// </summary>
        /// <param name="order">The order.</param>
        public RenderViewComponentProjection(int order)
        {
            Order = order;
        }

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
        /// Renders an action projection to given html output.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="securityService"></param>
        /// <param name="html">The html helper.</param>
        /// <param name="componentHelper">The View Component helper</param>
        /// <returns><c>true</c> on success, otherwise <c>false</c>.</returns>
        public bool Render(IPage page, ISecurityService securityService, IHtmlHelper html, IViewComponentHelper componentHelper)
        {
            if (AccessRole != null && !securityService.IsAuthorized(AccessRole))
            {
                return false;
            }

            componentHelper.RenderInvoke<TViewComponent>();
            
            return true;
        }
    }
}