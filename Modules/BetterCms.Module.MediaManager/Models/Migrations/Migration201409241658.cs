using System;

using BetterModules.Core.DataAccess.DataContext.Migrations;
using BetterCms.Module.Root.Models.Migrations;

using FluentMigrator;

namespace BetterCms.Module.MediaManager.Models.Migrations
{
    [Migration(201409241658)]
    public class Migration201409241658: DefaultMigration
    {
        private readonly string rootModuleSchemaName;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="201409241658"/> class.
        /// </summary>
        public Migration201409241658()
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
                       Id = new Guid("d88604ee-d3d5-4e0c-b071-984143a9ed74"),
                       Version = 1,
                       CreatedOn = DateTime.Now,
                       ModifiedOn = DateTime.Now,
                       CreatedByUser = "Admin",
                       ModifiedByUser = "Admin",
                       IsDeleted = 0,
                       Identifier = "media-images-url",
                       Title = "Image URL"
                   });
        }        
    }
}