using BetterCms.Module.Newsletter.Services;
using BetterCms.Module.Newsletter.ViewModels;

using BetterCms.Module.Root.Mvc;

using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.Newsletter.Command.SaveSubscriber
{
    public class SaveSubscriberCommand : CommandBase, ICommand<SubscriberViewModel, SubscriberViewModel>
    {
        /// <summary>
        /// Gets or sets the subscriber service.
        /// </summary>
        /// <value>
        /// The subscriber service.
        /// </value>
        public ISubscriberService SubscriberService { get; set; }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public SubscriberViewModel Execute(SubscriberViewModel request)
        {
            var subscriber = SubscriberService.SaveSubscriber(request.Email, request.Id, request.Version, request.IgnoreUniqueSubscriberException);

            return new SubscriberViewModel
            {
                Id = subscriber.Id,
                Version = subscriber.Version,
                Email = subscriber.Email
            };
        }
    }
}