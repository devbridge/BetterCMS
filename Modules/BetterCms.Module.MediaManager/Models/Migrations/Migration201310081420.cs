using System;

using BetterModules.Core.DataAccess.DataContext.Migrations;
using BetterCms.Module.Root.Models.Migrations;

using FluentMigrator;

namespace BetterCms.Module.MediaManager.Models.Migrations
{
    [Migration(201310081420)]
    public class Migration201310081420: DefaultMigration
    {
        private readonly string rootModuleSchemaName;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201308252344"/> class.
        /// </summary>
        public Migration201310081420()
            : base(MediaManagerModuleDescriptor.ModuleName)
        {
            rootModuleSchemaName = (new RootVersionTableMetaData()).SchemaName;
        }

        /// <summary>
        /// Ups this instance.
        /// </summary>
        public override void Up()
        {            
            // Create custom option for selecting a media folder
            Insert
               .IntoTable("CustomOptions").InSchema(rootModuleSchemaName)
               .Row(new
                   {
                       Id = new Guid("FB118858-CD1F-4CC6-8C22-177652EEB2A7"),
                       Version = 1,
                       CreatedOn = DateTime.Now,
                       ModifiedOn = DateTime.Now,
                       CreatedByUser = "Admin",
                       ModifiedByUser = "Admin",
                       IsDeleted = 0,
                       Identifier = "media-images-folder",
                       Title = "Image Folder"
                   });
        }        
    }
}