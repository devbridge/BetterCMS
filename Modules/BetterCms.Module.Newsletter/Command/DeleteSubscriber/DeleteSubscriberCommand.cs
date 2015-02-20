using BetterCms.Module.Newsletter.Models;
using BetterCms.Module.Newsletter.ViewModels;

using BetterCms.Module.Root.Mvc;

using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.Newsletter.Command.DeleteSubscriber
{
    public class DeleteSubscriberCommand : CommandBase, ICommand<SubscriberViewModel, bool>
    {
        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>True</c>, if newsletter subscriber is deleted successfully.</returns>
        public bool Execute(SubscriberViewModel request)
        {
            var subscriber = Repository.Delete<Subscriber>(request.Id, request.Version);
            UnitOfWork.Commit();

            Events.NewsletterEvents.Instance.OnSubscriberDeleted(subscriber);

            return true;
        }
    }
}