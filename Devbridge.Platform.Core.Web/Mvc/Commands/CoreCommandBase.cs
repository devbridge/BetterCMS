using Devbridge.Platform.Core.DataAccess;
using Devbridge.Platform.Core.DataAccess.DataContext;

namespace Devbridge.Platform.Core.Web.Mvc.Commands
{
    public abstract class CoreCommandBase : ICommandBase
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
    }
}
