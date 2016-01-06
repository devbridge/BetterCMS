using System;

using BetterModules.Core.DataAccess.DataContext.Migrations;

using FluentMigrator;

namespace BetterCms.Module.Users.Models.Migrations
{
    [Migration(201309091554)]
    public class Migration201309091554: DefaultMigration
    {
        private static readonly Guid loginWidgetId = new Guid("DE0E47B2-728D-4BE6-904D-ED99CDDEDA4A");

        private readonly string rootSchemaName = (new Root.Models.Migrations.RootVersionTableMetaData()).SchemaName;

        public Migration201309091554()
            : base(UsersModuleDescriptor.ModuleName)
        {
        }
        
        public override void Up()
        {
            Update.Table("Contents").InSchema(rootSchemaName)
                .Set(new { IsDeleted = true })
                .Where(new { Id = loginWidgetId });
        }
    }
}