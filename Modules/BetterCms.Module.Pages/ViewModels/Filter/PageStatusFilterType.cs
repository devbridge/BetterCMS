namespace BetterCms.Module.Pages.ViewModels.Filter
{
    public enum PageStatusFilterType
    {
        /// <summary>
        /// Only published pages
        /// </summary>
        OnlyPublished = 1,

        /// <summary>
        /// Only unpublished pages
        /// </summary>
        OnlyUnpublished = 2,

        /// <summary>
        /// Pages, containing unpublished contents
        /// </summary>
        ContainingUnpublishedContents = 3
    }
}