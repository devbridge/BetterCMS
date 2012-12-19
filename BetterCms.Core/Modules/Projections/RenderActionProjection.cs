using System;
using System.Linq.Expressions;
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

        public void Render(IPage page, HtmlHelper html)
        {            
            html.RenderAction(htmlActionExpression);
        }
    }
}