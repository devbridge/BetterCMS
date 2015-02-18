using System;

using Devbridge.Platform.Core.DataAccess.DataContext.Migrations;
using Devbridge.Platform.Core.Models;

using FluentMigrator;

namespace Devbridge.Platform.Sample.Module.Models.Migrations
{
    /// <summary>
    /// Module initial database structure creation.
    /// </summary>
    [Migration(201502181430)]
    public class InitialSetup : DefaultMigration
    {
        public InitialSetup()
            : base(SampleModuleDescriptor.ModuleName)
        {            
        }

        public override void Up()
        {
            Create
                 .Table("TestTable").InSchema(SchemaName)
                 .WithBaseColumns()
                 .WithColumn("Name").AsString(MaxLength.Name).NotNullable()
                 .WithColumn("Description").AsString(MaxLength.Text).NotNullable();
        }
    }
}