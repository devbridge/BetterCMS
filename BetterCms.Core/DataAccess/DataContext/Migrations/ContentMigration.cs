namespace BetterCms.Core.DataAccess.DataContext.Migrations
{
    class ContentMigration
    {
        public string ModuleName { get; private set; }
        public long MigrationVersion { get; private set; }
        public IContentMigration Migration { get; private set; }

        public ContentMigration(string moduleName, long migrationVersion, IContentMigration migration)
        {
            ModuleName = moduleName;
            MigrationVersion = migrationVersion;
            Migration = migration;
        }
    }
}
