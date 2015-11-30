// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISitemapsService.cs" company="Devbridge Group LLC">
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
namespace BetterCms.Module.Api.Operations.Pages.Sitemaps
{
    /// <summary>
    /// Service contract for sitemaps.
    /// </summary>
    public interface ISitemapsService
    {
        /// <summary>
        /// Gets the sitemaps list.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>GetSitemapsResponse</c> with tags list.</returns>
        GetSitemapsResponse Get(GetSitemapsRequest request);

        // NOTE: do not implement: replaces all the sitemaps.
        // PutSitemapsResponse Put(PutSitemapsRequest request);

        /// <summary>
        /// Creates a new sitemap.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>PostSitemapResponse</c> with a new sitemap id.</returns>
        PostSitemapResponse Post(PostSitemapRequest request);

        // NOTE: do not implement: drops all the sitemaps.
        // DeleteSitemapsResponse Delete(DeleteSitemapsRequest request);
    }
}