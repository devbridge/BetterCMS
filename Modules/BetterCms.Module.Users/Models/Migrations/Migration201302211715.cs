using System;

using BetterCms.Core.DataAccess.DataContext.Migrations;
using BetterCms.Core.Models;

using FluentMigrator;

namespace BetterCms.Module.Users.Models.Migrations
{
    /// <summary>
    /// Module database structure update.
    /// </summary>
    [Migration(201302211715)]
    public class Migration201302211715 : DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201302211715"/> class.
        /// </summary>
        public Migration201302211715()
            : base(UsersModuleDescriptor.ModuleName)
        {
        }

        /// <summary>
        /// Ups this instance.
        /// </summary>
        public override void Up()
        {
            Alter.Table("Users").InSchema(SchemaName).AlterColumn("Password").AsAnsiString(MaxLength.Password).NotNullable();
            Alter.Table("Users").InSchema(SchemaName).AddColumn("Salt").AsAnsiString(MaxLength.Password).NotNullable();
        }

        /// <summary>
        /// Downs this instance.
        /// </summary>
        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}