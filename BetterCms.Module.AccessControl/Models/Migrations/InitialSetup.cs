using System;

using BetterCms.Core.DataAccess.DataContext.Migrations;
using BetterCms.Core.Models;

using FluentMigrator;

namespace BetterCms.Module.AccessControl.Models.Migrations
{
    /// <summary>
    /// Module initial database structure creation.
    /// </summary>
    [Migration(201307302059)]
    public class InitialSetup : DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InitialSetup"/> class.
        /// </summary>
        public InitialSetup()
            : base(UserAccessModuleDescriptor.ModuleName.ToLowerInvariant())
        {
        }

        /// <summary>
        /// Ups this instance.
        /// </summary>
        public override void Up()
        {
            CreateUserAccessTable();
        }

        /// <summary>
        /// Downs this instance.
        /// </summary>
        public override void Down()
        {
            throw new NotImplementedException();
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
                .WithColumn("User").AsString(MaxLength.Name).NotNullable()
                .WithColumn("AccessLevel").AsInt32().NotNullable();
        }
    }
}