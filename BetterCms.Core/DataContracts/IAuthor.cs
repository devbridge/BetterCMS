namespace BetterCms.Core.DataContracts
{
    public interface IAuthor : IEntity
    {
        string Name { get; }

        IMediaImage Image { get; }
    }
}
