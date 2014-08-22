namespace BetterCms.Module.Api.Operations.Root.Languages.Language
{
    /// <summary>
    /// Contract for language service.
    /// </summary>
    public interface ILanguageService
    {
        /// <summary>
        /// Gets the specified language.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>GetLanguageRequest</c> with an language.</returns>
        GetLanguageResponse Get(GetLanguageRequest request);

        /// <summary>
        /// Replaces the language or if it doesn't exist, creates it.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>PutLanguageResponse</c> with a language id.</returns>
        PutLanguageResponse Put(PutLanguageRequest request);

        // NOTE: do not implement: should treat the addressed member as a collection in its own right and create a new entry in it.
        // PostLanguageResponse Post(PostLanguageRequest request);

        /// <summary>
        /// Deletes the specified language.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>DeleteLanguageResponse</c> with success status.</returns>
        DeleteLanguageResponse Delete(DeleteLanguageRequest request);
    }
}