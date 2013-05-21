using System;
using System.Collections.Generic;

using BetterCms.Core.DataAccess.DataContext.Migrations;

using FluentMigrator;

namespace BetterCms.Module.Templates.Models.Migrations
{
    /// <summary>
    /// Module initial database structure creation.
    /// </summary>
    [Migration(201301151842)]
    public class InitialSetup : DefaultMigration
    {
        /// <summary>
        /// The regions table name.
        /// </summary>
        private const string RegionsTableName = "Regions";

        /// <summary>
        /// The layouts table name.
        /// </summary>
        private const string LayoutsTableName = "Layouts";

        /// <summary>
        /// The layout regions table name.
        /// </summary>
        private const string LayoutRegionsTableName = "LayoutRegions";

        /// <summary>
        /// The root schema name.
        /// </summary>
        private readonly string rootSchemaName;

        /// <summary>
        /// Initializes a new instance of the <see cref="InitialSetup"/> class.
        /// </summary>
        public InitialSetup()
            : base(TemplatesModuleDescriptor.ModuleName)
        {
            rootSchemaName = (new Root.Models.Migrations.RootVersionTableMetaData()).SchemaName;
        }

        /// <summary>
        /// Ups this instance.
        /// </summary>
        public override void Up()
        {
            if (IsFirstTimeMigration())
            {
                CreateLayouts();
                CreateRegions();
                CreateLayoutRegions();
            }
            else
            {
                UpdateLayoutRegions(false);
                UpdateLayouts(false);
                UpdateRegions(false);
            }
        }

        /// <summary>
        /// Downs this instance.
        /// </summary>
        public override void Down()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Determines whether it is first time migration.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if it is first time migration; otherwise, <c>false</c>.
        /// </returns>
        private bool IsFirstTimeMigration()
        {
            return Schema.Schema(SchemaName).Table(TemplatesVersionTableMetaData.VersionInfoTableName).Exists();
        }

        /// <summary>
        /// Creates the layouts.
        /// </summary>
        private void CreateLayouts()
        {
            foreach (var layout in GetLayouts())
            {
                Insert
                    .IntoTable(LayoutsTableName)
                    .InSchema(rootSchemaName)
                    .Row(layout);
            }
        }

        /// <summary>
        /// Creates the regions.
        /// </summary>
        private void CreateRegions()
        {
            foreach (var region in GetRegions())
            {
                Insert
                    .IntoTable(RegionsTableName)
                    .InSchema(rootSchemaName)
                    .Row(region);
            }
        }

        /// <summary>
        /// Creates the layout regions.
        /// </summary>
        private void CreateLayoutRegions()
        {
            foreach (var layoutRegion in GetLayoutRegions())
            {
                Insert
                    .IntoTable(LayoutRegionsTableName)
                    .InSchema(rootSchemaName)
                    .Row(layoutRegion);
            }
        }

        /// <summary>
        /// Updates the layouts.
        /// </summary>
        /// <param name="delete">if set to <c>true</c> to mark layout as deleted.</param>
        private void UpdateLayouts(bool delete)
        {
            foreach (var layout in GetLayouts())
            {
                Update
                    .Table(LayoutsTableName)
                    .InSchema(rootSchemaName)
                    .Set(new Deleted(delete))
                    .Where(new { Id = layout.Id });
            }
        }

        /// <summary>
        /// Updates the regions.
        /// </summary>
        /// <param name="delete">if set to <c>true</c> to mark region as deleted.</param>
        private void UpdateRegions(bool delete)
        {
            foreach (var region in GetRegions())
            {
                Update
                    .Table(RegionsTableName)
                    .InSchema(rootSchemaName)
                    .Set(new Deleted(delete))
                    .Where(new { Id = region.Id });
            }
        }

        /// <summary>
        /// Updates the layout regions.
        /// </summary>
        /// <param name="delete">if set to <c>true</c> to mark layout region as deleted.</param>
        private void UpdateLayoutRegions(bool delete)
        {
            if (delete)
            {
                foreach (var region in GetRegions())
                {
                    Update
                        .Table(LayoutRegionsTableName)
                        .InSchema(rootSchemaName)
                        .Set(new Deleted(true))
                        .Where(new { RegionId = region.Id });
                }

                foreach (var layout in GetLayouts())
                {
                    Update
                        .Table(LayoutRegionsTableName)
                        .InSchema(rootSchemaName)
                        .Set(new Deleted(true))
                        .Where(new { LayoutId = layout.Id });
                }
            }
            else
            {
                foreach (var layoutRegion in GetLayoutRegions())
                {
                    Update
                        .Table(LayoutRegionsTableName)
                        .InSchema(rootSchemaName)
                        .Set(new Deleted(false))
                        .Where(new { LayoutId = layoutRegion.LayoutId, RegionId = layoutRegion.RegionId });
                }
            }
        }

        /// <summary>
        /// Gets the layouts.
        /// </summary>
        /// <returns>Layout list.</returns>
        private List<Layout> GetLayouts()
        {
            var layouts = new List<Layout>();

            layouts.Add(new Layout
            {
                Id = TemplatesModuleConstants.TemplateIds.Wide,
                LayoutPath = "~/Areas/bcms-templates/Views/Shared/WideLayout.cshtml",
                Name = "Default Wide"
            });

            layouts.Add(new Layout
            {
                Id = TemplatesModuleConstants.TemplateIds.TwoColumns,
                LayoutPath = "~/Areas/bcms-templates/Views/Shared/TwoColumnsLayout.cshtml",
                Name = "Default Two Columns"
            });

            layouts.Add(new Layout
            {
                Id = TemplatesModuleConstants.TemplateIds.ThreeColumns,
                LayoutPath = "~/Areas/bcms-templates/Views/Shared/ThreeColumnsLayout.cshtml",
                Name = "Default Three Columns"
            });

            return layouts;
        }

        /// <summary>
        /// Gets the regions.
        /// </summary>
        /// <returns>Region list.</returns>
        private List<Region> GetRegions()
        {
            var regions = new List<Region>();

            regions.Add(new Region
            {
                Id = TemplatesModuleConstants.RegionIds.MainContent,
                RegionIdentifier = "CMSMainContent"
            });

            regions.Add(new Region
            {
                Id = TemplatesModuleConstants.RegionIds.Header,
                RegionIdentifier = "CMSHeader"
            });

            regions.Add(new Region
            {
                Id = TemplatesModuleConstants.RegionIds.Footer,
                RegionIdentifier = "CMSFooter"
            });

            regions.Add(new Region
            {
                Id = TemplatesModuleConstants.RegionIds.LeftSide,
                RegionIdentifier = "CMSLeftSide"
            });

            regions.Add(new Region
            {
                Id = TemplatesModuleConstants.RegionIds.RightSide,
                RegionIdentifier = "CMSRightSide"
            });

            return regions;
        }

        /// <summary>
        /// Gets the layout regions.
        /// </summary>
        /// <returns>Layout region list.</returns>
        private List<LayoutRegion> GetLayoutRegions()
        {
            var layoutRegions = new List<LayoutRegion>();

            layoutRegions.Add(new LayoutRegion { Description = "Main Content", LayoutId = TemplatesModuleConstants.TemplateIds.Wide, RegionId = TemplatesModuleConstants.RegionIds.MainContent });
            layoutRegions.Add(new LayoutRegion { Description = "Header", LayoutId = TemplatesModuleConstants.TemplateIds.Wide, RegionId = TemplatesModuleConstants.RegionIds.Header });
            layoutRegions.Add(new LayoutRegion { Description = "Footer", LayoutId = TemplatesModuleConstants.TemplateIds.Wide, RegionId = TemplatesModuleConstants.RegionIds.Footer });

            layoutRegions.Add(new LayoutRegion { Description = "Main Content", LayoutId = TemplatesModuleConstants.TemplateIds.TwoColumns, RegionId = TemplatesModuleConstants.RegionIds.MainContent });
            layoutRegions.Add(new LayoutRegion { Description = "Header", LayoutId = TemplatesModuleConstants.TemplateIds.TwoColumns, RegionId = TemplatesModuleConstants.RegionIds.Header });
            layoutRegions.Add(new LayoutRegion { Description = "Footer", LayoutId = TemplatesModuleConstants.TemplateIds.TwoColumns, RegionId = TemplatesModuleConstants.RegionIds.Footer });
            layoutRegions.Add(new LayoutRegion { Description = "Left Side", LayoutId = TemplatesModuleConstants.TemplateIds.TwoColumns, RegionId = TemplatesModuleConstants.RegionIds.LeftSide });

            layoutRegions.Add(new LayoutRegion { Description = "Main Content", LayoutId = TemplatesModuleConstants.TemplateIds.ThreeColumns, RegionId = TemplatesModuleConstants.RegionIds.MainContent });
            layoutRegions.Add(new LayoutRegion { Description = "Header", LayoutId = TemplatesModuleConstants.TemplateIds.ThreeColumns, RegionId = TemplatesModuleConstants.RegionIds.Header });
            layoutRegions.Add(new LayoutRegion { Description = "Footer", LayoutId = TemplatesModuleConstants.TemplateIds.ThreeColumns, RegionId = TemplatesModuleConstants.RegionIds.Footer });
            layoutRegions.Add(new LayoutRegion { Description = "Left Side", LayoutId = TemplatesModuleConstants.TemplateIds.ThreeColumns, RegionId = TemplatesModuleConstants.RegionIds.LeftSide });
            layoutRegions.Add(new LayoutRegion { Description = "Right Side", LayoutId = TemplatesModuleConstants.TemplateIds.ThreeColumns, RegionId = TemplatesModuleConstants.RegionIds.RightSide });

            return layoutRegions;
        }
    }

    #region Internal Data Classes

    internal class Deleted
    {
        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
        public string DeletedByUser { get; set; }

        public Deleted(bool deleted)
        {
            IsDeleted = deleted;
            if (deleted)
            {
                DeletedOn = DateTime.Now;
                DeletedByUser = "Admin";
            }
        }
    }

    internal abstract class BaseModel
    {
        public Guid Id { get; set; }
        public int Version { get; set; }
        public string CreatedByUser { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedByUser { get; set; }
        public DateTime ModifiedOn { get; set; }
        public bool IsDeleted { get; set; }

        public BaseModel()
        {
            ModifiedByUser = CreatedByUser = "Admin";
            ModifiedOn = CreatedOn = DateTime.Now;
            IsDeleted = false;
            Version = 1;
            Id = Guid.NewGuid();
        }
    }

    internal class Layout : BaseModel
    {
        public string Name { get; set; }
        public string LayoutPath { get; set; }
    }

    internal class Region : BaseModel
    {        
        public string RegionIdentifier { get; set; }
    }

    internal class LayoutRegion : BaseModel
    {
        public string Description { get; set; }
        public Guid LayoutId { get; set; }
        public Guid RegionId { get; set; }
    }

    #endregion
}