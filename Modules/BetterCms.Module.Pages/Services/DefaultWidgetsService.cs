using BetterCms.Core.DataAccess.DataContext;

namespace BetterCms.Module.Pages.Services
{
    internal class DefaultWidgetsService : IWidgetsService
    {
        /// <summary>
        /// The unit of work
        /// </summary>
        private IUnitOfWork unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultWidgetsService" /> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public DefaultWidgetsService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }       
    }
}