using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;

using BetterCms.Configuration;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Core.Security;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.ViewModels.Security;

namespace BetterCms.Module.Root.Commands.Authentication.SearchUsers
{
    /// <summary>
    /// A command to get user list by filter.
    /// </summary>
    public class SearchUsersCommand : CommandBase, ICommand<string, List<LookupKeyValue>>
    {
        /// <summary>
        /// The configuration.
        /// </summary>
        private readonly ICmsConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchUsersCommand"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public SearchUsersCommand(ICmsConfiguration configuration)
        {
            this.configuration = configuration;
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
                .Where(accessRule => !accessRule.IsForRole && accessRule.Identity.ToLower().Contains(userName.ToLower()))
                .Select(accessRule => accessRule.Identity)
                .ToList();

            result.AddRange(Membership.GetAllUsers()
                .Cast<MembershipUser>()
                .Where(a => a.UserName.ToLower().Contains(userName.ToLower()))
                .Select(a => a.UserName)
                .ToList());

            return result
                .OrderBy(user => user)
                .Select(user => new LookupKeyValue { Key = user, Value = user })
                .ToList();
        }
    }
}