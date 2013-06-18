using System.Linq;

using BetterCms.Api;
using BetterCms.Core;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Blog.ViewModels.Author;
using BetterCms.Module.MediaManager.ViewModels;

using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Grids.Extensions;
using BetterCms.Module.Root.Mvc.Grids.GridOptions;
using BetterCms.Module.Root.ViewModels.SiteSettings;

namespace BetterCms.Module.Blog.Commands.GetAuthorList
{
    public class GetAuthorListCommand : CommandBase, ICommand<SearchableGridOptions, SearchableGridViewModel<AuthorViewModel>>
    {
        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>List with blog post view models</returns>
        public SearchableGridViewModel<AuthorViewModel> Execute(SearchableGridOptions request)
        {
            using (var api = CmsContext.CreateApiContextOf<BlogsApiContext>())
            {
                var query = api.GetAuthorsAsQueryable();
                var authors =
                    query.Select(
                        author =>
                        new AuthorViewModel
                        {
                            Id = author.Id,
                            Version = author.Version,
                            Name = author.Name,
                            Image =
                                !author.ImageId.HasValue
                                    ? null
                                    : new ImageSelectorViewModel
                                    {
                                        ImageId = author.ImageId,
                                        ImageVersion = author.ImageVersion,
                                        ImageUrl = author.ImagePublicUrl,
                                        ThumbnailUrl = author.ImagePublicThumbnailUrl,
                                        ImageTooltip = author.ImageCaption
                                    }
                        });

                if (!string.IsNullOrWhiteSpace(request.SearchQuery))
                {
                    authors = authors.Where(a => a.Name.Contains(request.SearchQuery));
                }

                request.SetDefaultSortingOptions("Name");
                var count = authors.ToRowCountFutureValue();
                authors = authors.AddSortingAndPaging(request);

                return new SearchableGridViewModel<AuthorViewModel>(authors.ToList(), request, count.Value);
            }
        }
    }
}