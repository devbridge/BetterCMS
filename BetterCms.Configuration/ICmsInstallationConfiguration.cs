namespace BetterCms
{
    /// <summary>
    /// Installation module configuration contract.
    /// </summary>
    public interface ICmsInstallationConfiguration
    {
        /// <summary>
        /// Gets or sets a value indicating whether to install 404 error page.
        /// </summary>
        /// <value>
        ///   <c>true</c> to install 404 error page; otherwise, <c>false</c>.
        /// </value>
        bool Install404ErrorPage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to install 500 error page.
        /// </summary>
        /// <value>
        ///   <c>true</c> to install 500 error page; otherwise, <c>false</c>.
        /// </value>
        bool Install500ErrorPage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to install default page.
        /// </summary>
        /// <value>
        ///   <c>true</c> to install default page; otherwise, <c>false</c>.
        /// </value>
        bool InstallDefaultPage { get; set; }
    }
}
