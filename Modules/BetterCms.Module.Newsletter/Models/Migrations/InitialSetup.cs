using System;

using BetterModules.Core.DataAccess.DataContext.Migrations;
using BetterModules.Core.Models;

using FluentMigrator;

namespace BetterCms.Module.Newsletter.Models.Migrations
{
    /// <summary>
    /// Module initial database structure creation.
    /// </summary>
    [Migration(201304221200)]
    public class InitialSetup : DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InitialSetup"/> class.
        /// </summary>
        public InitialSetup()
            : base(NewsletterModuleDescriptor.ModuleName)
        {
        }

        /// <summary>
        /// Ups this instance.
        /// </summary>
        public override void Up()
        {
            CreateSubscribers();
        }

        /// <summary>
        /// Downs this instance.
        /// </summary>
        public override void Down()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates the subscribers.
        /// </summary>
        private void CreateSubscribers()
        {
            Create
                .Table("Subscribers").InSchema(SchemaName)
                .WithBaseColumns()
                .WithColumn("Email").AsString(MaxLength.Email).NotNullable();
        }
    }
}