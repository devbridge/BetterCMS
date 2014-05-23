namespace BetterCms.Module.Api.Operations.Pages.Contents.Content.HtmlContent
{
    /// <summary>
    /// Html content service contract for REST.
    /// </summary>
    public interface IHtmlContentService
    {
        /// <summary>
        /// Gets the specified html content.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        ///   <c>GetHtmlContentResponse</c> with html content.
        /// </returns>
        GetHtmlContentResponse Get(GetHtmlContentRequest request);

        /// <summary>
        /// Puts the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        ///   <c>PutHtmlContentResponse</c> with html content id.
        /// </returns>
        PutHtmlContentResponse Put(PutHtmlContentRequest request);
        
        /// <summary>
        /// Posts the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        ///   <c>PostHtmlContentResponse</c> with html content id.
        /// </returns>
        PostHtmlContentResponse Post(PostHtmlContentRequest request);

        /// <summary>
        /// Deletes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>DeleteHtmlContentResponse</c> with html content id.</returns>
        DeleteHtmlContentResponse Delete(DeleteHtmlContentRequest request);
    }
}