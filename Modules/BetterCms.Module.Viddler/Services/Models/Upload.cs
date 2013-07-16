namespace BetterCms.Module.Viddler.Services.Models
{
    /// <summary>
    /// Upload data model for Viddler service.
    /// </summary>
    public class Upload
    {
        /// <summary>
        /// Gets or sets the session id.
        /// </summary>
        /// <value>
        /// The session id.
        /// </value>
        public string SessionId { get; set; }

        /// <summary>
        /// Gets or sets the token.
        /// </summary>
        /// <value>
        /// The token.
        /// </value>
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets the endpoint.
        /// </summary>
        /// <value>
        /// The endpoint.
        /// </value>
        public string Endpoint { get; set; }
    }
}