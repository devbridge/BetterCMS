// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultMediaFileUrlResolver.cs" company="Devbridge Group LLC">
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

using BetterCms.Module.MediaManager.Controllers;

using BetterModules.Core.Web.Web;

namespace BetterCms.Module.MediaManager.Services
{
    public class DefaultMediaFileUrlResolver : IMediaFileUrlResolver
    {
        /// <summary>
        /// The context accessor
        /// </summary>
        private readonly IHttpContextAccessor contextAccessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultMediaFileUrlResolver" /> class.
        /// </summary>
        /// <param name="contextAccessor">The context accessor.</param>
        public DefaultMediaFileUrlResolver(IHttpContextAccessor contextAccessor)
        {
            this.contextAccessor = contextAccessor;
        }

        /// <summary>
        /// Gets the media file full URL.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="publicUrl">The public URL.</param>
        /// <returns></returns>
        public string GetMediaFileFullUrl(Guid id, string publicUrl)
        {
            return contextAccessor.ResolveActionUrl<FilesController>(f => f.Download(id.ToString()), true);
        }

        public string EnsureFullPathUrl(string url)
        {
            if (!string.IsNullOrEmpty(url) && url.StartsWith("/", StringComparison.Ordinal))
            {
                return contextAccessor.MapPublicPath(url);
            }

            return url;
        }
    }
}