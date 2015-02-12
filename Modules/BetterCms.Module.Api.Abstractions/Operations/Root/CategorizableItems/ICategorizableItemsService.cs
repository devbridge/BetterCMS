namespace BetterCms.Module.Api.Operations.Root.CategorizableItems
{
    /// <summary>
    /// Service contract for categorizable items.
    /// </summary>
    public interface ICategorizableItemsService
    {
        /// <summary>
        /// Gets the categorizable items.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>GetCategorizableItemsResponse</c> with categorizable items list</returns>
        GetCategorizableItemsResponse Get(GetCategorizableItemsRequest request);
    }
}
