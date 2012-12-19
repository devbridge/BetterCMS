using System;

namespace BetterCms.Core.Models
{
    /// <summary>
    /// Defines interface to access a basic page content properties.
    /// </summary>
    public interface IPageContent
    {
        Guid Id { get; }

        IPage Page { get; }

        IContent Content { get; }

        IRegion Region { get; }
    }
}
