using System.Transactions;

using BetterCms.Core;
using BetterCms.Core.DataAccess.DataContext.Migrations;

namespace BetterCms.Module.Root.Models.MigrationsContent
{
    public abstract class ContentMigrationBase : IContentMigration
    {
        public abstract void Up(ICmsConfiguration configuration);

        public virtual bool IsUpNeeded(string moduleName, long contentVersion)
        {
            using (var api = CmsContext.CreateApiContextOf<MigratorApiContext>())
            {
                return !api.IsContentMigrated(moduleName, contentVersion);
            }
        }

        public virtual void UpPerformed(string moduleName, long contentVersion)
        {
            using (var rootApi = CmsContext.CreateApiContextOf<MigratorApiContext>())
            {
                using (var transactionScope = new TransactionScope())
                {
                    rootApi.ContentMigrated(moduleName, contentVersion);
                    transactionScope.Complete();
                }
            }
        }
    }
}