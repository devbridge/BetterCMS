using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;

using BetterCms.Configuration;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.ViewModels.Autocomplete;

using BetterModules.Core.Web.Mvc.Commands;
using BetterModules.Core.Web.Services.Caching;

namespace BetterCms.Module.Root.Commands.Authentication.SearchRoles
{
    /// <summary>
    /// A command to get roles list by filter.
    /// </summary>
    public class SearchRolesCommand : CommandBase, ICommand<SuggestionViewModel, List<LookupKeyValue>>
    {
        /// <summary>
        /// The cache key.
        /// </summary>
        private const string CacheKey = "bcms-rolesforautocomplete";

        /// <summary>
        /// The configuration.
        /// </summary>
        private readonly ICmsConfiguration configuration;

        /// <summary>
        /// The cache service.
        /// </summary>
        private readonly ICacheService cacheService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchRolesCommand" /> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="cacheService">The cache service.</param>
        public SearchRolesCommand(ICmsConfiguration configuration, ICacheService cacheService)
        {
            this.configuration = configuration;
            this.cacheService = cacheService;
        }

        /// <summary>
        /// Executes this command.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// A list of roles.
        /// </returns>
        public List<LookupKeyValue> Execute(SuggestionViewModel model)
        {
            var allRoleNames = cacheService.Get(CacheKey, TimeSpan.FromSeconds(30), GetAllRoleNames);

            var query = allRoleNames
                .Where(role => role.ToLower().Contains(model.Query.ToLower()));

            if (model.ExistingItemsArray.Length > 0)
            {
                query = query.Where(role => !model.ExistingItems.Contains(role));
            }

            return query
                .Select(role => new LookupKeyValue { Key = role, Value = role })
                .ToList();
        }

        /// <summary>
        /// Gets all role names.
        /// </summary>
        /// <returns>Distinct ordered role list.</returns>
        private IList<string> GetAllRoleNames()
        {
            var result = new List<string>();

            if (Roles.Enabled)
            {
                // Add roles from roles provider
                result.AddRange(Roles.Provider.GetAllRoles()
                    .Distinct()
                    .ToList());
            }
            else
            {
                // Add roles from access rules table
                result.AddRange(
                    Repository.AsQueryable<AccessRule>()
                        .Where(a => a.IsForRole)
                        .Select(a => a.Identity)
                        .Distinct()
                        .ToList());
            }

            // Add default roles from configuration
            foreach (AccessControlElement userAccess in configuration.Security.DefaultAccessRules)
            {
                if (userAccess.IsRole)
                {
                    result.Add(userAccess.Identity);
                }
            }

            return result
                .Distinct()
                .OrderBy(role => role)
                .ToList();
        }
    }
}