// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SearchUsersCommand.cs" company="Devbridge Group LLC">
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
using System.Linq;
using System.Web.Security;

using BetterCms.Configuration;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.ViewModels.Autocomplete;

using Common.Logging;

using BetterModules.Core.Web.Mvc.Commands;
using BetterModules.Core.Web.Services.Caching;

namespace BetterCms.Module.Root.Commands.Authentication.SearchUsers
{
    /// <summary>
    /// A command to get user list by filter.
    /// </summary>
    public class SearchUsersCommand : CommandBase, ICommand<SuggestionViewModel, List<LookupKeyValue>>
    {
        /// <summary>
        /// The cache key.
        /// </summary>
        private const string CacheKey = "bcms-usersforautocomplete";

        /// <summary>
        /// Current class logger.
        /// </summary>
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// The configuration.
        /// </summary>
        private readonly ICmsConfiguration configuration;

        /// <summary>
        /// The cache service.
        /// </summary>
        private readonly ICacheService cacheService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchUsersCommand" /> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="cacheService">The cache service.</param>
        public SearchUsersCommand(ICmsConfiguration configuration, ICacheService cacheService)
        {
            this.configuration = configuration;
            this.cacheService = cacheService;
        }

        /// <summary>
        /// Executes this command.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// A list of users.
        /// </returns>
        public List<LookupKeyValue> Execute(SuggestionViewModel model)
        {
            var allUserNames = cacheService.Get(CacheKey, TimeSpan.FromSeconds(30), GetAllUserNames);

            var query = allUserNames
                .Where(user => user.ToLower().Contains(model.Query.ToLower()));

            if (model.ExistingItemsArray.Length > 0)
            {
                query = query.Where(user => !model.ExistingItems.Contains(user));
            }

            return query
                .Select(user => new LookupKeyValue { Key = user, Value = user })
                .ToList();
        }

        /// <summary>
        /// Gets all user names.
        /// </summary>
        /// <returns>Distinct ordered username list.</returns>
        private IList<string> GetAllUserNames()
        {
            var result = new List<string>();

            try
            {
                // Add users from membership provider
                result.AddRange(
                    Membership.GetAllUsers()
                        .Cast<MembershipUser>()
                        .Select(a => a.UserName)
                        .Distinct()
                        .ToList());
            }
            catch (Exception ex)
            {
                Log.Error("Error occurred while retrieving users.", ex);
            }

            if (!result.Any())
            {
                // Add users from access rules table
                result.AddRange(
                    Repository.AsQueryable<AccessRule>()
                        .Where(a => !a.IsForRole)
                        .Select(a => a.Identity)
                        .Distinct()
                        .ToList());
            }

            // Add default users from configuration
            foreach (AccessControlElement userAccess in configuration.Security.DefaultAccessRules)
            {
                if (!userAccess.IsRole)
                {
                    result.Add(userAccess.Identity);
                }
                
            }

            return result
                .Distinct()
                .OrderBy(user => user)
                .ToList();
        }
    }
}