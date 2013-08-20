using BetterCms.Core.DataAccess.DataContext.Migrations;
using BetterCms.Core.Models;

using FluentMigrator;

namespace BetterCms.Module.Root.Models.Migrations
{
    [Migration(201308121549)]
    public class Migration201308121549 : DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201308121549"/> class.
        /// </summary>
        public Migration201308121549()
            : base(RootModuleDescriptor.ModuleName)
        {
        }

        public override void Up()
        {
            CreateUserAccessTable();
        }

        /// <summary>
        /// Creates the user access table.
        /// </summary>
        private void CreateUserAccessTable()
        {
            Create
                .Table("UserAccess")
                .InSchema(SchemaName)
                .WithCmsBaseColumns()
                .WithColumn("ObjectId").AsGuid().NotNullable()
                .WithColumn("RoleOrUser").AsString(MaxLength.Name).NotNullable()
                .WithColumn("AccessLevel").AsInt32().NotNullable();
        }
    }
}