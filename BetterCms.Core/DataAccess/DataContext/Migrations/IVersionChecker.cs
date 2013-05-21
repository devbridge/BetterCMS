namespace BetterCms.Core.DataAccess.DataContext.Migrations
{
    public interface IVersionChecker
    {
        bool VersionExists(string moduleName, long version);

        void AddVersion(string moduleName, long version);
    }
}
