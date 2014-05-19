using BetterCms.Module.Api.Operations.Root.Languages.Language;

namespace BetterCms.Module.Api.Operations.Root.Languages
{
    /// <summary>
    /// Language service contract for REST.
    /// </summary>
    public interface ILanguagesService
    {
        /// <summary>
        /// Gets languages list.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>GetLanguagesResponse</c> with languages list.</returns>
        GetLanguagesResponse Get(GetLanguagesRequest request);


        // NOTE: do not implement: replaces all the languages.
        // PutTagsResponse Put(PutTagsRequest request);

        /// <summary>
        /// Creates a new language.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>PostLanguagesResponse</c> with a new language id.</returns>
        PostLanguageResponse Post(PostLanguageRequest request);

        // NOTE: do not implement: drops all the languages.
        // DeleteLanguagesResponse Delete(DeleteLanguagesRequest request);
    }
}
