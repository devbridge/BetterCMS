using BetterCms.Module.Api.Operations.Root.Categories.Category.Nodes;
using BetterCms.Module.Api.Operations.Root.Categories.Category.Nodes.Node;
using BetterCms.Module.Api.Operations.Root.Categories.Category.Tree;

namespace BetterCms.Module.Api.Operations.Root.Categories.Category
{
    /// <summary>
    /// Service contract for category operations.
    /// </summary>
    public interface ICategoryService
    {
        /// <summary>
        /// Gets the tree.
        /// </summary>
        /// <value>
        /// The tree.
        /// </value>
        ICategoryTreeService Tree { get; }

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
        /// Gets the category specified in request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>GetCategoriesResponse</c> with category data.</returns>
        GetCategoryResponse Get(GetCategoryRequest request);

        /// <summary>
        /// Puts the category specified in request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>PutCategoriesResponse</c> with updated category data.</returns>
        PutCategoryResponse Put(PutCategoryRequest request);

        /// <summary>
        /// Deletes the category specified in request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>DeleteCategoriesResponse</c> with success status.</returns>
        DeleteCategoryResponse Delete(DeleteCategoryRequest request);
    }
}