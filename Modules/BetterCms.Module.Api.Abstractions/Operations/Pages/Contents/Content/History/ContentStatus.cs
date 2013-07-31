using System;

namespace BetterCms.Module.Api.Operations.Pages.Contents.Content.History
{
    /// <summary>
    /// Indicates content publishing status (auto saved for preview, draft, published, archived).
    /// </summary>
    [Serializable]
    public enum ContentStatus
    {
        Preview = 1,
        Draft = 2,
        Published = 3,
        Archived = 4
    }
}
