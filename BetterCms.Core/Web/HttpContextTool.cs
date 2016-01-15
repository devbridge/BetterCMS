// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HttpContextTool.cs" company="Devbridge Group LLC">
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
using System.Web;

namespace BetterCms.Core.Web
{
    /// <summary>
    /// TODO: Move to helpers
    /// </summary>
    public class HttpContextTool
    {
        private readonly HttpContextBase httpContext;

        public HttpContextTool(HttpContextBase httpContext)
        {
            this.httpContext = httpContext;
        }

        public string GetAbsolutePath(string virtualPath = null)
        {
            var request = httpContext.Request;
            var originalUrl = virtualPath ?? request.ServerVariables["HTTP_X_ORIGINAL_URL"];
            var host = request.Url != null ? request.Url.Host : null;

            if (!string.IsNullOrEmpty(originalUrl))
            {
                return new Uri(string.Format("http://{0}{1}", host, originalUrl), UriKind.Absolute).AbsolutePath;
            }

            var originalUri = new Uri(string.Format("http://{0}{1}", host, request.RawUrl));
            var path = originalUri.AbsolutePath;

            if (request.PathInfo.Length > 0)
            {
                path = path.Substring(0, path.Length - request.PathInfo.Length);
            }

            return path;
        }
    }
}
