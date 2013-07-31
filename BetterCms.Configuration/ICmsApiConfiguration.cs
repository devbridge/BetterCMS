namespace BetterCms
{
    /// <summary>
    /// API configuration contract.
    /// </summary>
    public interface ICmsApiConfiguration
    {
        /// <summary>
        /// Gets a value indicating whether web API enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if web API enabled; otherwise, <c>false</c>.
        /// </value>
        bool WebApiDisabled { get; }
    }
}