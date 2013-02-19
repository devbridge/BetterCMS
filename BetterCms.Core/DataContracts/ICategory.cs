namespace BetterCms.Core.DataContracts
{
    /// <summary>
    /// Defines interface to access basic content properties.
    /// </summary>
    public interface ICategory : IEntity
    {
        string Name { get; }
    }
}
