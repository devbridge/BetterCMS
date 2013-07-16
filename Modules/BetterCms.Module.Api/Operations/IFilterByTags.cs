namespace BetterCms.Module.Api.Operations
{
    public interface IFilterByTags
    {
        /// <summary>
        /// Gets or sets the tags.
        /// </summary>
        /// <value>
        /// The tags.
        /// </value>
        System.Collections.Generic.List<string> FilterByTags { get; }

        /// <summary>
        /// Gets or sets the tags filter connector.
        /// </summary>
        /// <value>
        /// The tags filter connector.
        /// </value>
        Enums.FilterConnector FilterByTagsConnector { get; }
    }
}