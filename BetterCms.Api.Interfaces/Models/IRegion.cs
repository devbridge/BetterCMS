namespace BetterCms.Api.Interfaces.Models
{
    /// <summary>
    /// Defines interface to access basic content properties.
    /// </summary>
    public interface IRegion : IEntity
    {       
        string RegionIdentifier { get; }
    }
}
