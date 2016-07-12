// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IRedirectService.cs" company="Devbridge Group LLC">
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
using System.Collections.Generic;

using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.ViewModels.SiteSettings;

namespace BetterCms.Module.Pages.Services
{
    public interface IRedirectService
    {
        /// <summary>
        /// Checks if such redirect doesn't exists yet and creates new redirect entity.
        /// </summary>
        /// <param name="pageUrl">The page URL.</param>
        /// <param name="redirectUrl">The redirect URL.</param>
        /// <returns>
        /// New created redirect entity or null, if such redirect already exists
        /// </returns>
        Redirect CreateRedirectEntity(string pageUrl, string redirectUrl);

        /// <summary>
        /// Validates urls: checks if the the circular loop exists.
        /// </summary>
        /// <param name="pageUrl">The page URL.</param>
        /// <param name="redirectUrl">The redirect URL.</param>
        /// <param name="id">The id.</param>
        void ValidateForCircularLoop(string pageUrl, string redirectUrl, Guid? id = null);

        /// <summary>
        /// Checks, if such redirect exists.
        /// </summary>
        /// <param name="pageUrl">The page URL.</param>
        /// <param name="id">The redirect id.</param>
        /// <returns>
        /// true, if redirect exists, false if not exists
        /// </returns>
        void ValidateRedirectExists(string pageUrl, Guid? id = null);

        /// <summary>
        /// Gets the redirect.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>Redirect url</returns>
        string GetRedirect(string url);

        /// <summary>
        /// Gets redirect if such redirect exists.
        /// </summary>
        /// <param name="pageUrl">The page URL.</param>
        /// <param name="id">The redirect Id.</param>
        /// <returns>
        /// Redirect entity or null, if such redirect doesn't already exists
        /// </returns>
        Redirect GetPageRedirect(string pageUrl, Guid? id = null);

        /// <summary>
        /// Saves the redirect.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="createIfNotExists">if set to <c>true</c> creates entity if such not exists.</param>
        /// <returns>Saved redirect entity</returns>
        Redirect SaveRedirect(SiteSettingRedirectViewModel model, bool createIfNotExists = false);

        /// <summary>
        /// Deletes the redirect.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="version">The version.</param>
        /// <returns>Deletion result</returns>
        bool DeleteRedirect(Guid id, int version);


        /// <summary>
        /// Gets all redirects.
        /// </summary>
        /// <returns>List of redirects</returns>
        IList<Redirect> GetAllRedirects();
    }
}