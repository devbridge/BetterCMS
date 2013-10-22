using BetterCms.Core.DataAccess;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Root.Mvc.Helpers;

namespace BetterCms.Module.Blog.Services
{
    public class DefaultBlogService : IBlogService
    {
        /// <summary>
        /// The cms configuration
        /// </summary>
        private readonly ICmsConfiguration configuration;

        /// <summary>
        /// The url service
        /// </summary>
        private readonly IUrlService urlService;

        /// <summary>
        /// The repository
        /// </summary>
        private readonly IRepository repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultBlogService" /> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="urlService">The URL service.</param>
        /// <param name="repository">The repository.</param>
        public DefaultBlogService(ICmsConfiguration configuration, IUrlService urlService, IRepository repository)
        {
            this.configuration = configuration;
            this.urlService = urlService;
            this.repository = repository;
        }

        /// <summary>
        /// Creates the blog URL from the given blog title.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <returns>Created blog URL</returns>
        public string CreateBlogPermalink(string title)
        {
            var url = title.Transliterate(true);
            url = urlService.AddPageUrlPostfix(url, configuration.ArticleUrlPattern);

            return url;
        }       
    }
}