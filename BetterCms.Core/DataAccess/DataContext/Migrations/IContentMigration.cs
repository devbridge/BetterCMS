namespace BetterCms.Core.DataAccess.DataContext.Migrations
{
    public interface IContentMigration
    {
        bool IsUpNeeded(string moduleName, long contentVersion);
        void Up(ICmsConfiguration configuration);
        void UpPerformed(string moduleName, long contentVersion);
    }
}
