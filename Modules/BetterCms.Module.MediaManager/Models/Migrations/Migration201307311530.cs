using BetterCms.Core.DataAccess.DataContext.Migrations;

using FluentMigrator;

namespace BetterCms.Module.MediaManager.Models.Migrations
{
    /// <summary>
    /// Migration for MediaFolderDependency table.
    /// </summary>
    [Migration(201307311530)]
    public class Migration201307311530 : DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201307311530"/> class.
        /// </summary>
        public Migration201307311530()
            : base(MediaManagerModuleDescriptor.ModuleName)
        {
        }

        /// <summary>
        /// Migrate up.
        /// </summary>
        public override void Up()
        {
            if (!Schema.Schema(SchemaName).Table("MediaFolderDependencies").Exists())
            {
                Create.Table("MediaFolderDependencies")
                      .InSchema(SchemaName)
                      .WithCmsBaseColumns()
                      .WithColumn("ParentId")
                      .AsGuid()
                      .Nullable()
                      .WithColumn("ChildId")
                      .AsGuid()
                      .Nullable();

                Create.ForeignKey("FK_MediaFolderDependencies_ParentId_MediaFolders_Id")
                      .FromTable("MediaFolderDependencies")
                      .InSchema(SchemaName)
                      .ForeignColumn("ParentId")
                      .ToTable("MediaFolders")
                      .InSchema(SchemaName)
                      .PrimaryColumn("Id");

                Create.ForeignKey("FK_MediaFolderDependencies_ChildId_MediaFolders_Id")
                      .FromTable("MediaFolderDependencies")
                      .InSchema(SchemaName)
                      .ForeignColumn("ChildId")
                      .ToTable("MediaFolders")
                      .InSchema(SchemaName)
                      .PrimaryColumn("Id");

                Create.UniqueConstraint("Uq_MediaFolderDependencies_ParentId_ChildId")
                      .OnTable("MediaFolderDependencies")
                      .WithSchema(SchemaName)
                      .Columns(new[] { "ChildId", "ParentId" });

                IfSqlServer().Execute.EmbeddedScript("Migration201307311530.sqlserver.sql");

                // TODO: add Postgres support.
                IfPostgres().Execute.Sql(PostgresThrowNotSupportedErrorSql);

                // TODO: add Oracle support.
                IfOracle().Execute.Sql(OracleThrowNotSupportedErrorSql);
            }
        }
    }
}