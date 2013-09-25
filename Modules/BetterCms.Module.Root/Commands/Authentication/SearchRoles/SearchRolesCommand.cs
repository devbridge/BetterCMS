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

namespace BetterCms.Module.Root.Commands.Authentication.SearchRoles
{
    /// <summary>
    /// A command to get roles list by filter.
    /// </summary>
    public class SearchRolesCommand : CommandBase, ICommand<string, List<LookupKeyValue>>
    {
        /// <summary>
        /// The configuration.
        /// </summary>
        private readonly ICmsConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchRolesCommand"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public SearchRolesCommand(ICmsConfiguration configuration)
        {
            this.configuration = configuration;
        }

        /// <summary>
        /// Executes this command.
        /// </summary>
        /// <param name="roleName">Name of the role.</param>
        /// <returns>
        /// A list of roles.
        /// </returns>
        public List<LookupKeyValue> Execute(string roleName)
        {
            if (!Roles.Enabled)
            {
                return null;
            }

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
                .Where(accessRule => accessRule.IsForRole && accessRule.Identity.ToLower().Contains(roleName.ToLower()))
                .Select(accessRule => accessRule.Identity)
                .ToList();

            result.AddRange(Roles.Provider.GetAllRoles()
                .Where(role => role.ToLower().Contains(roleName.ToLower()))
                .ToList());

            return result
                .OrderBy(role => role)
                .Select(role => new LookupKeyValue { Key = role, Value = role })
                .ToList();
        }
    }
}