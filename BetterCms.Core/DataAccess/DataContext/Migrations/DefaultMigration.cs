using FluentMigrator;

namespace BetterCms.Core.DataAccess.DataContext.Migrations
{
    public abstract class DefaultMigration : Migration
    {
        private readonly string moduleName;

        public string SchemaName
        {
            get
            {
                return "bcms_" + moduleName;
            }
        }

        public DefaultMigration(string moduleName)
        {
            this.moduleName = moduleName;
        }
    }
}
