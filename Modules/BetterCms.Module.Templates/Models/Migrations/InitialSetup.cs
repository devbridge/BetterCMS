using System;
using System.Collections.Generic;

using BetterCms.Core.DataAccess.DataContext.Migrations;

using FluentMigrator;

namespace BetterCms.Module.Templates.Models.Migrations
{
    [Migration(201301151842)]
    public class InitialSetup : DefaultMigration
    {
        private const string RegionsTableName = "Regions";

        private const string LayoutsTableName = "Layouts";

        private const string LayoutRegionsTableName = "LayoutRegions";

        private readonly string rootSchemaName;

        public InitialSetup()
            : base(TemplatesModuleDescriptor.ModuleName)
        {
            rootSchemaName = (new Root.Models.Migrations.RootVersionTableMetaData()).SchemaName;
        }

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

        public override void Down()
        {
            UpdateLayoutRegions(true);
            UpdateLayouts(true);
            UpdateRegions(true);
        }

        private bool IsFirstTimeMigration()
        {
            return Schema.Schema(SchemaName).Table(TemplatesVersionTableMetaData.VersionInfoTableName).Exists();
        }

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

        private List<Layout>  GetLayouts()
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

    public class Deleted
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

    public abstract class BaseModel
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

    public class Layout : BaseModel
    {
        public string Name { get; set; }
        public string LayoutPath { get; set; }
    }

    public class Region : BaseModel
    {        
        public string RegionIdentifier { get; set; }
    }
    
    public class LayoutRegion : BaseModel
    {
        public string Description { get; set; }
        public Guid LayoutId { get; set; }
        public Guid RegionId { get; set; }
    }
}