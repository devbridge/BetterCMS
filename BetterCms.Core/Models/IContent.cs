using System;

namespace BetterCms.Core.Models
{
    /// <summary>
    /// Defines interface to access basic content properties.
    /// </summary>
    public interface IContent
    {
        Guid Id { get; set; }
    }
}
