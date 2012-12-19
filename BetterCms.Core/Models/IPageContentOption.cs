using System;

namespace BetterCms.Core.Models
{
    /// <summary>
    /// Defines interface to access the page content option.
    /// </summary>
    public interface IPageContentOption
    {
        Guid Id { get; }

        IPageContent PageContent { get; }

        IContentOption ContentOption { get; }

        string Value { get; }
    }
}
