using BetterCms.Module.Api.Operations.Root.Categories.Category.Nodes;
using BetterCms.Module.Api.Operations.Root.Categories.Category.Nodes.Node;
using BetterCms.Module.Api.Operations.Root.Categories.Category.Tree;
using BetterCms.Module.Api.Operations.Root.CategorizableItems;

namespace BetterCms.Module.Api.Operations.Root.Categories.Category
{
    /// <summary>
    /// Service contract for category operations.
    /// </summary>
    public interface ICategoryTreeService
    {
        /// <summary>
        /// Gets the tree.
        /// </summary>
        /// <value>
        /// The tree.
        /// </value>
        INodesTreeService Tree { get; }

        /// <summary>
        /// Gets the nodes.
        /// </summary>
        /// <value>
        /// The nodes.
        /// </value>
        INodesService Nodes { get; }

        /// <summary>
        /// Gets the node.
        /// </summary>
        /// <value>
        /// The node.
        /// </value>
        INodeService Node { get; }

        /// <summary>
        /// Gets the categorizable items.
        /// </summary>
        /// <value>
        /// The categorizable items.
        /// </value>
        ICategorizableItemsService CategorizableItems { get; }

        /// <summary>
        /// Gets the category specified in request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>GetCategoriesResponse</c> with category data.</returns>
        GetCategoryTreeResponse Get(GetCategoryTreeRequest request);

        /// <summary>
        /// Puts the category specified in request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>PutCategoriesResponse</c> with updated category data.</returns>
        PutCategoryTreeResponse Put(PutCategoryTreeRequest request);

        /// <summary>
        /// Deletes the category specified in request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>DeleteCategoriesResponse</c> with success status.</returns>
        DeleteCategoryTreeResponse Delete(DeleteCategoryTreeRequest request);
    }
}