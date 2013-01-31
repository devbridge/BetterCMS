using System;

using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Module.Blog.Models;
using BetterCms.Module.Root.Models;

namespace BetterCms.Module.Blog.Services
{
    public class DefaultOptionService : IOptionService
    {
        /// <summary>
        /// The unit of work
        /// </summary>
        private IUnitOfWork unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultOptionService" /> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public DefaultOptionService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Gets the default template id.
        /// </summary>
        /// <returns>
        /// Default template id or null, if such is not set
        /// </returns>
        public Guid? GetDefaultTemplateId()
        {
            Option optionAlias = null;
            Layout layoutAlias = null;

            var options = unitOfWork.Session
                .QueryOver(() => optionAlias)
                .Left.JoinQueryOver(() => optionAlias.DefaultLayout, () => layoutAlias)
                .Where(() => !optionAlias.IsDeleted)
                .OrderBy(() => optionAlias.CreatedOn).Desc
                .Select(select => select.DefaultLayout.Id)
                .Take(1)
                .List<Guid>();

            if (options != null && options.Count > 0)
            {
                return options[0];
            }

            return null;
        }
    }
}