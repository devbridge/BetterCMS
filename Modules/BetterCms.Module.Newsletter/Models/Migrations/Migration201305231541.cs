// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Migration201305231541.cs" company="Devbridge Group LLC">
// 
// Copyright (C) 2015,2016 Devbridge Group LLC
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with this program.  If not, see http://www.gnu.org/licenses/. 
// </copyright>
// 
// <summary>
// Better CMS is a publishing focused and developer friendly .NET open source CMS.
// 
// Website: https://www.bettercms.com 
// GitHub: https://github.com/devbridge/bettercms
// Email: info@bettercms.com
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using System;

using BetterModules.Core.DataAccess.DataContext.Migrations;
using BetterCms.Module.Pages.Models.Migrations;
using BetterCms.Module.Root.Models.Migrations;

using FluentMigrator;

namespace BetterCms.Module.Newsletter.Models.Migrations
{
    [Migration(201305231541)]
    public class Migration201305231541: DefaultMigration
    {
        private readonly string rootSchemaName;

        private readonly string pagesSchemaName;

        public Migration201305231541()
            : base(NewsletterModuleDescriptor.ModuleName)
        {
            rootSchemaName = (new RootVersionTableMetaData()).SchemaName;
            pagesSchemaName = (new PagesVersionTableMetaData()).SchemaName;
        }
        
        public override void Up()
        {
            var subscribeToNewsletterWidget = new
                                                  {

                                                      ForRootSchemaContentTable = new
                                                                                      {
                                                                                          Id = "87B734F8-2B28-49E8-B8FC-3B2982AAB796",
                                                                                          Version = 1,
                                                                                          IsDeleted = false,
                                                                                          CreatedOn = DateTime.Now,
                                                                                          CreatedByUser = "Better CMS",
                                                                                          ModifiedOn = DateTime.Now,
                                                                                          ModifiedByUser = "Better CMS",
                                                                                          Name = "Newsletter Widget",
                                                                                          Status = 3,
                                                                                          PublishedOn = DateTime.Now,
                                                                                          PublishedByUser = "Better CMS"
                                                                                      },

                                                      ForRootScemaWidgetsTable = new
                                                                                     {
                                                                                         Id = "87B734F8-2B28-49E8-B8FC-3B2982AAB796",
                                                                                     },

                                                      ForPagesSchemaServerControlWidgetsTable = new
                                                                                                    {
                                                                                                        Id = "87B734F8-2B28-49E8-B8FC-3B2982AAB796",
                                                                                                        Url = "~/Areas/bcms-newsletter/Views/Widgets/SubscribeToNewsletter.cshtml"
                                                                                                    }

                                                  };

            var options = new {
                                  Option1 = new {
                                                    Id = "6AB1E8E2-2F9E-40A3-ABE0-5A2B18FB2BF6",
                                                    Version = 1,
                                                    IsDeleted = false,
                                                    CreatedOn = DateTime.Now,
                                                    CreatedByUser = "Better CMS",
                                                    ModifiedOn = DateTime.Now,
                                                    ModifiedByUser = "Better CMS",
                                                    ContentId = subscribeToNewsletterWidget.ForRootSchemaContentTable.Id,
                                                    Key = "Email placeholder",
                                                    Type = 1, // Text
                                                    DefaultValue = "email..."
                                                },

                                  Option2 = new {
                                                    Id = "1F9D6700-1276-4715-BAFA-9937CD02C5C9",
                                                    Version = 1,
                                                    IsDeleted = false,
                                                    CreatedOn = DateTime.Now,
                                                    CreatedByUser = "Better CMS",
                                                    ModifiedOn = DateTime.Now,
                                                    ModifiedByUser = "Better CMS",
                                                    ContentId = subscribeToNewsletterWidget.ForRootSchemaContentTable.Id,
                                                    Key = "Label title",
                                                    Type = 1, // Text
                                                    DefaultValue = "Subscribe to newsletter"
                                                },

                                  Option3 = new {
                                                    Id = "410374CC-A83F-4257-8A47-7CB4D514A925",
                                                    Version = 1,
                                                    IsDeleted = false,
                                                    CreatedOn = DateTime.Now,
                                                    CreatedByUser = "Better CMS",
                                                    ModifiedOn = DateTime.Now,
                                                    ModifiedByUser = "Better CMS",
                                                    ContentId = subscribeToNewsletterWidget.ForRootSchemaContentTable.Id,
                                                    Key = "Submit title",
                                                    Type = 1, // Text
                                                    DefaultValue = "Submit"
                                                },

                                  Option4 = new {
                                                    Id = "33741162-AB86-4E62-A91C-6F7C2772D8F4",
                                                    Version = 1,
                                                    IsDeleted = false,
                                                    CreatedOn = DateTime.Now,
                                                    CreatedByUser = "Better CMS",
                                                    ModifiedOn = DateTime.Now,
                                                    ModifiedByUser = "Better CMS",
                                                    ContentId = subscribeToNewsletterWidget.ForRootSchemaContentTable.Id,
                                                    Key = "Submit is disabled",
                                                    Type = 1, // Text
                                                    DefaultValue = "false"
                                                }
                              };

            // Register server control widget.
            Insert.IntoTable("Contents").InSchema(rootSchemaName).Row(subscribeToNewsletterWidget.ForRootSchemaContentTable);
            Insert.IntoTable("Widgets").InSchema(rootSchemaName).Row(subscribeToNewsletterWidget.ForRootScemaWidgetsTable);
            Insert.IntoTable("ServerControlWidgets").InSchema(pagesSchemaName).Row(subscribeToNewsletterWidget.ForPagesSchemaServerControlWidgetsTable);                      

            // Add widget options.
            Insert.IntoTable("ContentOptions").InSchema(rootSchemaName)
                .Row(options.Option1)
                .Row(options.Option2)
                .Row(options.Option3)
                .Row(options.Option4);                
        }
    }
}