namespace BetterCms.Core.DataContracts
{
    public interface IRedirect : IEntity
    {
        string PageUrl { get; }

        string RedirectUrl { get; }
    }
}
