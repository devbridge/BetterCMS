using System.Transactions;

using BetterCms.Api;
using BetterCms.Core;
using BetterCms.Core.DataAccess.DataContext.Migrations;

namespace BetterCms.Module.Root.Models.MigrationsContent
{
    public abstract class BaseContentMigration : IContentMigration
    {
        public abstract void Up(ICmsConfiguration configuration);

        public virtual bool IsUpNeeded(string moduleName, long contentVersion)
        {
            using (var rootApi = CmsContext.CreateApiContextOf<RootApiContext>())
            {
                return !rootApi.IsContentMigrated(moduleName, contentVersion);
            }
        }

        public virtual void UpPerformed(string moduleName, long contentVersion)
        {
            using (var rootApi = CmsContext.CreateApiContextOf<RootApiContext>())
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