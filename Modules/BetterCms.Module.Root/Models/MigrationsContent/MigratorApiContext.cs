using System;
using System.Linq;

using Autofac;

using BetterCms.Api;
using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.Exceptions.Api;

namespace BetterCms.Module.Root.Models.MigrationsContent
{
    public class MigratorApiContext : DataApiContext
    {
        public MigratorApiContext(ILifetimeScope lifetimeScope, IRepository repository = null, IUnitOfWork unitOfWork = null)
            : base(lifetimeScope, repository, unitOfWork)
        {
        }

        public bool IsContentMigrated(string moduleName, long contentVersion)
        {
            try
            {
                return Repository.AsQueryable<ModuleContentVersion>(v => v.ModuleName == moduleName && v.ContentVersion == contentVersion).Any();
            }
            catch (Exception ex)
            {
                const string message = "Failed to perform version check.";
                Logger.Error(message, ex);
                throw new CmsApiException(message, ex);
            }
        }

        public void ContentMigrated(string moduleName, long contentVersion)
        {
            try
            {
                var version = new ModuleContentVersion { ModuleName = moduleName, ContentVersion = contentVersion };
                Repository.Save(version);

                UnitOfWork.Commit();
            }
            catch (Exception ex)
            {
                const string message = "Failed to perform version saving.";
                Logger.Error(message, ex);
                throw new CmsApiException(message, ex);
            }
        }
    }
}