using System;

namespace BetterCms.Module.Pages.DataContracts.Enums
{
    /// <summary>
    /// Enumerator with flags, determining, which Page children referenced and collections must be loaded.
    /// </summary>
    [Flags]
    public enum PageLoadableChilds
    {
        /// <summary>
        /// No child references and collections.
        /// </summary>
        None = 0,

        /// <summary>
        /// The layout.
        /// </summary>
        Layout = 1 << 0,

        /// <summary>
        /// The layout region.
        /// </summary>
        LayoutRegion = 1 << 1,

        /// <summary>
        /// The tags.
        /// </summary>
        Tags = 1 << 2,

        /// <summary>
        /// The category.
        /// </summary>
        Category = 1 << 3,

        /// <summary>
        /// The image.
        /// </summary>
        Image = 1 << 4,

        /// <summary>
        /// All specified child referenced and collections.
        /// </summary>
        All = LayoutRegion | Tags | Category | Image
    }
}