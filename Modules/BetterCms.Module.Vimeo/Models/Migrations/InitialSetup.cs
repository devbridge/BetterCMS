using System;

using BetterCms.Core.DataAccess.DataContext.Migrations;

namespace BetterCms.Module.Vimeo.Models.Migrations
{
    /// <summary>
    /// Module initial database structure creation.
    /// </summary>
// TODO:    [Migration(201307111459)]
    public class InitialSetup : DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InitialSetup"/> class.
        /// </summary>
        public InitialSetup()
            : base(VimeoModuleDescriptor.ModuleName)
        {
        }

        /// <summary>
        /// Ups this instance.
        /// </summary>
        public override void Up()
        {
            throw new NotImplementedException();
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