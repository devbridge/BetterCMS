namespace BetterCms.Module.Api.Operations.Pages.Pages.Page.Properties
{
    /// <summary>
    /// Page properties service contract for REST.
    /// </summary>
    public interface IPagePropertiesService
    {
        /// <summary>
        /// Gets the specified page properties.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>GetPagePropertiesResponse</c> with a page properties data.</returns>
        GetPagePropertiesResponse Get(GetPagePropertiesRequest request);
    }
}