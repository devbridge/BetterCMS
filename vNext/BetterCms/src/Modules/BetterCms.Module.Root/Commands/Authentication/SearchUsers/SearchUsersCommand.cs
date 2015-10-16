using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Configuration;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.ViewModels.Autocomplete;
using BetterModules.Core.Infrastructure.Commands;
using Microsoft.Framework.Logging;
using Microsoft.Framework.OptionsModel;

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
        private readonly ILogger logger;

        /// <summary>
        /// The configuration.
        /// </summary>
        private readonly CmsConfigurationSection configuration;

        /// <summary>
        /// The cache service.
        /// </summary>
        private readonly ICacheService cacheService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchUsersCommand" /> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="cacheService">The cache service.</param>
        /// <param name="loggerFactory">The logger factory</param>
        public SearchUsersCommand(IOptions<CmsConfigurationSection> configuration, ICacheService cacheService, ILoggerFactory loggerFactory)
        {
            this.configuration = configuration.Value;
            this.cacheService = cacheService;
            logger = loggerFactory.CreateLogger<SearchUsersCommand>();
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
                logger.LogError("Error occurred while retrieving users.", ex);
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