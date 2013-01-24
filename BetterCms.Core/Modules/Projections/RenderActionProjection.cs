using System;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Web.Mvc;

using BetterCms.Core.Models;

using Microsoft.Web.Mvc;

namespace BetterCms.Core.Modules.Projections
{
    public class RenderActionProjection<TController> : IPageActionProjection where TController : Controller
    {
        private Expression<Action<TController>> htmlActionExpression;

        public RenderActionProjection(Expression<Action<TController>> htmlActionExpression)
        {
            this.htmlActionExpression = htmlActionExpression;
        }

        public RenderActionProjection(Expression<Action<TController>> htmlActionExpression, int order)
        {
            this.htmlActionExpression = htmlActionExpression;
            Order = order;
        }

        public int Order { get; set; }

        public Func<IPage, IPrincipal, bool> IsVisible { get; set; }

        public void Render(IPage page, IPrincipal principal, HtmlHelper html)
        {
            if (IsVisible != null && !IsVisible(page, principal))
            {
                return;
            }

            html.RenderAction(htmlActionExpression);
        }
    }
}