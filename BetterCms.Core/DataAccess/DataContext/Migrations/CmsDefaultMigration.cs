using Devbridge.Platform.Core.DataAccess.DataContext.Migrations;

namespace BetterCms.Core.DataAccess.DataContext.Migrations
{
    public abstract class CmsDefaultMigration : DefaultMigration
    {
        private readonly string moduleName;

        public override string SchemaName
        {
            get
            {
                return "bcms_" + moduleName;
            }
        }

        public CmsDefaultMigration(string moduleName) : base(moduleName)
        {
            this.moduleName = moduleName;
        }
    }
}
