using System;
using System.Linq.Expressions;
using System.Web.Mvc;

using BetterCms.Core.DataContracts;
using BetterCms.Core.Services;

using Microsoft.Web.Mvc;

namespace BetterCms.Core.Modules.Projections
{
    public class RenderActionProjection<TController> : IPageActionProjection where TController : Controller
    {
        /// <summary>
        /// The HTML action expression.
        /// </summary>
        private Expression<Action<TController>> htmlActionExpression;

        /// <summary>
        /// Initializes a new instance of the <see cref="RenderActionProjection{TController}"/> class.
        /// </summary>
        /// <param name="htmlActionExpression">The HTML action expression.</param>
        public RenderActionProjection(Expression<Action<TController>> htmlActionExpression)
        {
            this.htmlActionExpression = htmlActionExpression;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RenderActionProjection{TController}"/> class.
        /// </summary>
        /// <param name="htmlActionExpression">The HTML action expression.</param>
        /// <param name="order">The order.</param>
        public RenderActionProjection(Expression<Action<TController>> htmlActionExpression, int order)
        {
            this.htmlActionExpression = htmlActionExpression;
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
        /// <returns><c>true</c> on success, otherwise <c>false</c>.</returns>
        public bool Render(IPage page, ISecurityService securityService, HtmlHelper html)
        {
            if (AccessRole != null && !securityService.IsAuthorized(AccessRole))
            {
                return false;
            }

            html.RenderAction(htmlActionExpression);

            return true;
        }
    }
}