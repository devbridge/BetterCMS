using BetterCms.Module.Api.Operations.Pages.Pages.Page.Contents;
using BetterCms.Module.Api.Operations.Pages.Pages.Page.Contents.Content;
using BetterCms.Module.Api.Operations.Pages.Pages.Page.Exists;
using BetterCms.Module.Api.Operations.Pages.Pages.Page.Properties;
using BetterCms.Module.Api.Operations.Pages.Pages.Page.Translations;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page
{
    /// <summary>
    /// Page service contract for CRUD.
    /// </summary>
    public interface IPageService
    {
        /// <summary>
        /// Gets the properties.
        /// </summary>
        /// <value>
        /// The properties.
        /// </value>
        IPagePropertiesService Properties { get; }

        /// <summary>
        /// Gets the contents.
        /// </summary>
        /// <value>
        /// The contents.
        /// </value>
        IPageContentsService Contents { get; }

        /// <summary>
        /// Gets the translations.
        /// </summary>
        /// <value>
        /// The translations.
        /// </value>
        IPageTranslationsService Translations { get; }

        /// <summary>
        /// Gets the content.
        /// </summary>
        /// <value>
        /// The content.
        /// </value>
        IPageContentService Content { get; }

        /// <summary>
        /// Gets the page specified in request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>GetPageResponse</c> with page data.</returns>
        GetPageResponse Get(GetPageRequest request);

        /// <summary>
        /// Checks if specified page exists.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>PageExistsResponse</c> with page data.</returns>
        PageExistsResponse Exists(PageExistsRequest request);
    }
}