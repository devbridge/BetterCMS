using System.Collections.Generic;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Core.Security;
using BetterCms.Core.Services;
using BetterCms.Module.Root.Content.Resources;
using BetterCms.Module.Root.ViewModels.Security;

namespace BetterCms.Module.Root.Mvc
{
    /// <summary>
    /// A base class for commands.
    /// </summary>
    public abstract class CommandBase : ICommandBase
    {
        /// <summary>
        /// Gets or sets a command context.
        /// </summary>
        /// <value>
        /// A command executing context.
        /// </value>
        public ICommandContext Context { get; set; }

        /// <summary>
        /// Gets or sets the repository. This property is auto wired.
        /// </summary>
        /// <value>
        /// The repository.
        /// </value>
        public virtual IRepository Repository { get; set; }

        /// <summary>
        /// Gets or sets the unit of work. This property is auto wired.
        /// </summary>
        /// <value>
        /// The unit of work.
        /// </value>
        public virtual IUnitOfWork UnitOfWork { get; set; }

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