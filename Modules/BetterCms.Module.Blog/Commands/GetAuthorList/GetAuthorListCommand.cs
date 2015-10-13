using System.Linq;

using BetterCms.Module.Blog.Models;
using BetterCms.Module.Blog.Services;
using BetterCms.Module.Blog.ViewModels.Author;
using BetterCms.Module.MediaManager.Services;
using BetterCms.Module.MediaManager.ViewModels;

using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Grids.Extensions;
using BetterCms.Module.Root.Mvc.Grids.GridOptions;
using BetterCms.Module.Root.ViewModels.SiteSettings;

using BetterModules.Core.DataAccess.DataContext;
using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.Blog.Commands.GetAuthorList
{
    public class GetAuthorListCommand : CommandBase, ICommand<SearchableGridOptions, SearchableGridViewModel<AuthorViewModel>>
    {
        private IAuthorService authorService;

        private readonly IMediaFileUrlResolver fileUrlResolver;

        public GetAuthorListCommand(IAuthorService authorService, IMediaFileUrlResolver fileUrlResolver)
        {
            this.authorService = authorService;
            this.fileUrlResolver = fileUrlResolver;
        }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>List with blog post view models</returns>
        public SearchableGridViewModel<AuthorViewModel> Execute(SearchableGridOptions request)
        {
                var query = Repository.AsQueryable<Author>();
                var authors = query.Select(
                        author =>
                        new AuthorViewModel
                        {
                            Id = author.Id,
                            Version = author.Version,
                            Name = author.Name,
                            Description = author.Description,
                            Image = author.Image != null && !author.Image.IsDeleted
                                    ?
                                    new ImageSelectorViewModel
                                    {
                                        ImageId = author.Image.Id,
                                        ImageVersion = author.Image.Version,
                                        ImageUrl = fileUrlResolver.EnsureFullPathUrl(author.Image.PublicUrl),
                                        ThumbnailUrl = fileUrlResolver.EnsureFullPathUrl(author.Image.PublicThumbnailUrl),
                                        ImageTooltip = author.Image.Caption,
                                        FolderId = author.Image.Folder != null ? author.Image.Folder.Id : (System.Guid?)null
                                    }
                                    : null
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