using System.Collections.Generic;

using BetterCms.Core.Security;
using BetterCms.Core.Services;
using BetterCms.Module.Root.Content.Resources;
using BetterCms.Module.Root.ViewModels.Security;

namespace BetterCms.Module.Root.Mvc
{
    /// <summary>
    /// A base class for commands.
    /// </summary>
    public abstract class CommandBase : BetterModules.Core.Web.Mvc.Commands.CoreCommandBase
    {
        /// <summary>
        /// Gets or sets the security service.
        /// </summary>
        /// <value>
        /// The security service.
        /// </value>
        public virtual ISecurityService SecurityService { get; set; }

        /// <summary>
        /// Gets or sets the access control service.
        /// </summary>
        /// <value>
        /// The access control service.
        /// </value>
        public virtual IAccessControlService AccessControlService { get; set; }

        /// <summary>
        /// Checks for read only mode.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="rules">The rules.</param>
        protected void SetIsReadOnly(IAccessSecuredViewModel model, IList<IAccessRule> rules)
        {
            var principal = SecurityService.GetCurrentPrincipal();
            var accessLevel = AccessControlService.GetAccessLevel(rules, principal);

            model.IsReadOnly = accessLevel != AccessLevel.ReadWrite;

            if (model.IsReadOnly)
            {
                Context.Messages.AddInfo(RootGlobalization.Message_ReadOnlyMode);
            }
        }        
    }
}