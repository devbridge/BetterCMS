// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RenderActionProjection.cs" company="Devbridge Group LLC">
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