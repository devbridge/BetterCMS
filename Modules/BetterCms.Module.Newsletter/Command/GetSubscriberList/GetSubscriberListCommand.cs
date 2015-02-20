using System.Linq;

using BetterModules.Core.DataAccess.DataContext;
using BetterCms.Module.Newsletter.Models;
using BetterCms.Module.Newsletter.ViewModels;

using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Grids.Extensions;
using BetterCms.Module.Root.Mvc.Grids.GridOptions;
using BetterCms.Module.Root.ViewModels.SiteSettings;

using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.Newsletter.Command.GetSubscriberList
{
    public class GetSubscriberListCommand : CommandBase, ICommand<SearchableGridOptions, SearchableGridViewModel<SubscriberViewModel>>
    {
        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>List with blog post view models</returns>
        public SearchableGridViewModel<SubscriberViewModel> Execute(SearchableGridOptions request)
        {
            SearchableGridViewModel<SubscriberViewModel> model;

            request.SetDefaultSortingOptions("Email");

            var query = Repository
                .AsQueryable<Subscriber>();

            if (!string.IsNullOrWhiteSpace(request.SearchQuery))
            {
                query = query.Where(a => a.Email.Contains(request.SearchQuery));
            }

            var subscribers = query
                .Select(subscriber =>
                    new SubscriberViewModel
                    {
                        Id = subscriber.Id,
                        Version = subscriber.Version,
                        Email = subscriber.Email
                    });

            var count = query.ToRowCountFutureValue();
            subscribers = subscribers.AddSortingAndPaging(request);

            model = new SearchableGridViewModel<SubscriberViewModel>(subscribers.ToList(), request, count.Value);

            return model;
        }
    }
}