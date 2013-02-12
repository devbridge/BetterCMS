using System;

using BetterCms.Api.Interfaces.Models.Enums;

namespace BetterCms.Api.Interfaces.Models
{
    /// <summary>
    /// Defines interface to access a basic page content properties.
    /// </summary>
    public interface IHistorical : IEntity
    {
        ContentStatus Status { get; }        

        DateTime? PublishedOn { get; }
        
        string PublishedByUser { get; }
    }
}
