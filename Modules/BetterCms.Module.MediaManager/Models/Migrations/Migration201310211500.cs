using System;

using BetterModules.Core.DataAccess.DataContext.Migrations;
using BetterCms.Module.Root.Models.Migrations;

using FluentMigrator;

namespace BetterCms.Module.MediaManager.Models.Migrations
{
    [Migration(201310211500)]
    public class Migration201310211500: DefaultMigration
    {
        private readonly string rootModuleSchemaName;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201308252344"/> class.
        /// </summary>
        public Migration201310211500()
            : base(MediaManagerModuleDescriptor.ModuleName)
        {
            rootModuleSchemaName = (new RootVersionTableMetaData()).SchemaName;
        }

        /// <summary>
        /// Ups this instance.
        /// </summary>
        public override void Up()
        {
            // Update custom option title
            Update.Table("CustomOptions")
                  .InSchema(rootModuleSchemaName)
                  .Set(new { Title = "Images Folder" })
                  .Where(new { Id = new Guid("FB118858-CD1F-4CC6-8C22-177652EEB2A7") });
        }
    }
}