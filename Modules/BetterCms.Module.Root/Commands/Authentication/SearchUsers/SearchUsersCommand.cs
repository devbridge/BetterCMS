using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;

using BetterCms.Configuration;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Core.Security;
using BetterCms.Core.Services.Caching;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.ViewModels.Security;

using Common.Logging;

namespace BetterCms.Module.Root.Commands.Authentication.SearchUsers
{
    /// <summary>
    /// A command to get user list by filter.
    /// </summary>
    public class SearchUsersCommand : CommandBase, ICommand<string, List<LookupKeyValue>>
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
        /// <param name="userName">Name of the user.</param>
        /// <returns>
        /// A list of users.
        /// </returns>
        public List<LookupKeyValue> Execute(string userName)
        {
            var allUserNames = cacheService.Get(CacheKey, TimeSpan.FromSeconds(30), GetAllUserNames);

            return allUserNames
                .Where(user => user.ToLower().Contains(userName.ToLower()))
                .Select(user => new LookupKeyValue { Key = user, Value = user })
                .ToList();
        }

        /// <summary>
        /// Gets all user names.
        /// </summary>
        /// <returns>Distinct ordered username list.</returns>
        private IList<string> GetAllUserNames()
        {
            IList<IAccessRule> list = new List<IAccessRule>();
            foreach (AccessControlElement userAccess in configuration.Security.DefaultAccessRules)
            {
                list.Add(
                    new UserAccessViewModel
                    {
                        Identity = userAccess.Identity,
                        AccessLevel = (AccessLevel)Enum.Parse(typeof(AccessLevel), userAccess.AccessLevel),
                        IsForRole = userAccess.IsRole
                    });
            }

            var result = list
                .Where(accessRule => !accessRule.IsForRole)
                .Select(accessRule => accessRule.Identity)
                .Distinct()
                .ToList();

            try
            {
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

            result.AddRange(
                Repository.AsQueryable<AccessRule>()
                    .Where(a => !a.IsForRole)
                    .Select(a => a.Identity)
                    .Distinct()
                    .ToList());

            return result
                .Distinct()
                .OrderBy(user => user)
                .ToList();
        }
    }
}