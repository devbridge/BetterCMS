// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPagePropertiesService.cs" company="Devbridge Group LLC">
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
namespace BetterCms.Module.Api.Operations.Pages.Pages.Page.Properties
{
    /// <summary>
    /// Page properties service contract for REST.
    /// </summary>
    public interface IPagePropertiesService
    {
        /// <summary>
        /// Gets the specified page properties.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>GetPagePropertiesResponse</c> with a page properties data.</returns>
        GetPagePropertiesResponse Get(GetPagePropertiesRequest request);

        /// <summary>
        /// Puts the page specified in request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>PutPageResponse</c> with updated page id.</returns>
        PutPagePropertiesResponse Put(PutPagePropertiesRequest request);

        /// <summary>
        /// Posts the page specified in request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>PostPagePropertiesRequest</c> with updated page id.</returns>
        PostPagePropertiesResponse Post(PostPagePropertiesRequest request);

        /// <summary>
        /// Deletes the page specified in request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>DeletePageResponse</c> with success status.</returns>
        DeletePagePropertiesResponse Delete(DeletePagePropertiesRequest request);
    }
}