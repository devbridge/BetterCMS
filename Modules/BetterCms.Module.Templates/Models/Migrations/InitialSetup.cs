using System;
using System.Collections.Generic;

using BetterCms.Core.DataAccess.DataContext.Migrations;

using FluentMigrator;

namespace BetterCms.Module.Templates.Models.Migrations
{
    [Migration(1)]
    public class InitialSetup : DefaultMigration
    {
        private string rootSchemaName;

        private const string RegionsTableName = "Regions";
        
        private const string LayoutsTableName = "Layouts";

        private const string LayoutRegionsTableName = "LayoutRegions";

        public InitialSetup()
            : base(TemplatesModuleDescriptor.ModuleName)
        {
            rootSchemaName = (new Root.Models.Migrations.BlogVersionTableMetaData()).SchemaName;
        }

        public override void Up()
        {
            CreateLayouts();
            CreateRegions();
            CreateLayoutRegions();
        }

        public override void Down()
        {
            RemoveLayoutRegions();
            RemoveLayouts();
            RemoveRegions();
        }

        public void CreateLayouts()
        {
            foreach (var layout in GetLayouts())
            {
                Insert
                    .IntoTable(LayoutsTableName)
                    .InSchema(rootSchemaName)
                    .Row(layout);
            }
        }
        
        public void CreateRegions()
        {
            foreach (var region in GetRegions())
            {
                Insert
                    .IntoTable(RegionsTableName)
                    .InSchema(rootSchemaName)
                    .Row(region);
            }
        }

        public void CreateLayoutRegions()
        {
            foreach (var layoutRegion in GetLayoutRegions())
            {
                Insert
                    .IntoTable(LayoutRegionsTableName)
                    .InSchema(rootSchemaName)
                    .Row(layoutRegion);
            }
        }

        private void RemoveLayouts()
        {
            foreach (var layout in GetLayouts())
            {
                Delete
                    .FromTable(LayoutsTableName)
                    .InSchema(rootSchemaName)
                    .Row(new { Id = layout.Id });
            }
        }

        private void RemoveRegions()
        {
            foreach (var region in GetRegions())
            {
                Delete
                    .FromTable(RegionsTableName)
                    .InSchema(rootSchemaName)
                    .Row(new { Id = region.Id });
            }
        }

        private void RemoveLayoutRegions()
        {
            foreach (var region in GetRegions())
            {
                Delete
                    .FromTable(LayoutRegionsTableName)
                    .InSchema(rootSchemaName)
                    .Row(new { Id = region.Id });
            }

            foreach (var layout in GetLayouts())
            {
                Delete
                    .FromTable(LayoutRegionsTableName)
                    .InSchema(rootSchemaName)
                    .Row(new { Id = layout.Id });
            }
        }

        private List<Layout>  GetLayouts()
        {
            var layouts = new List<Layout>();

            layouts.Add(new Layout
            {
                Id = TemplatesModuleConstants.TemplateIds.Wide,
                LayoutPath = "~/Areas/bcms-Templates/Views/Shared/WideLayout.cshtml",
                Name = "Default Wide"
            });

            layouts.Add(new Layout
            {
                Id = TemplatesModuleConstants.TemplateIds.TwoColumns,
                LayoutPath = "~/Areas/bcms-Templates/Views/Shared/TwoColumnsLayout.cshtml",
                Name = "Default Two Columns"
            });

            layouts.Add(new Layout
            {
                Id = TemplatesModuleConstants.TemplateIds.ThreeColumns,
                LayoutPath = "~/Areas/bcms-Templates/Views/Shared/ThreeColumnsLayout.cshtml",
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
                Name = "Main Content",
                RegionIdentifier = "CMSMainContent"
            });

            regions.Add(new Region
            {
                Id = TemplatesModuleConstants.RegionIds.Header,
                Name = "Header",
                RegionIdentifier = "CMSHeader"
            });

            regions.Add(new Region
            {
                Id = TemplatesModuleConstants.RegionIds.Footer,
                Name = "Footer",
                RegionIdentifier = "CMSFooter"
            });

            regions.Add(new Region
            {
                Id = TemplatesModuleConstants.RegionIds.LeftSide,
                Name = "Left Side",
                RegionIdentifier = "CMSLeftSide"
            });

            regions.Add(new Region
            {
                Id = TemplatesModuleConstants.RegionIds.RightSide,
                Name = "Right Side",
                RegionIdentifier = "CMSRightSide"
            });

            return regions;
        }

        private List<LayoutRegion> GetLayoutRegions()
        {
            var layoutRegions = new List<LayoutRegion>();

            layoutRegions.Add(new LayoutRegion { LayoutId = TemplatesModuleConstants.TemplateIds.Wide, RegionId = TemplatesModuleConstants.RegionIds.MainContent });
            layoutRegions.Add(new LayoutRegion { LayoutId = TemplatesModuleConstants.TemplateIds.Wide, RegionId = TemplatesModuleConstants.RegionIds.Header });
            layoutRegions.Add(new LayoutRegion { LayoutId = TemplatesModuleConstants.TemplateIds.Wide, RegionId = TemplatesModuleConstants.RegionIds.Footer });

            layoutRegions.Add(new LayoutRegion { LayoutId = TemplatesModuleConstants.TemplateIds.TwoColumns, RegionId = TemplatesModuleConstants.RegionIds.MainContent });
            layoutRegions.Add(new LayoutRegion { LayoutId = TemplatesModuleConstants.TemplateIds.TwoColumns, RegionId = TemplatesModuleConstants.RegionIds.Header });
            layoutRegions.Add(new LayoutRegion { LayoutId = TemplatesModuleConstants.TemplateIds.TwoColumns, RegionId = TemplatesModuleConstants.RegionIds.Footer });
            layoutRegions.Add(new LayoutRegion { LayoutId = TemplatesModuleConstants.TemplateIds.TwoColumns, RegionId = TemplatesModuleConstants.RegionIds.LeftSide });

            layoutRegions.Add(new LayoutRegion { LayoutId = TemplatesModuleConstants.TemplateIds.ThreeColumns, RegionId = TemplatesModuleConstants.RegionIds.MainContent });
            layoutRegions.Add(new LayoutRegion { LayoutId = TemplatesModuleConstants.TemplateIds.ThreeColumns, RegionId = TemplatesModuleConstants.RegionIds.Header });
            layoutRegions.Add(new LayoutRegion { LayoutId = TemplatesModuleConstants.TemplateIds.ThreeColumns, RegionId = TemplatesModuleConstants.RegionIds.Footer });
            layoutRegions.Add(new LayoutRegion { LayoutId = TemplatesModuleConstants.TemplateIds.ThreeColumns, RegionId = TemplatesModuleConstants.RegionIds.LeftSide });
            layoutRegions.Add(new LayoutRegion { LayoutId = TemplatesModuleConstants.TemplateIds.ThreeColumns, RegionId = TemplatesModuleConstants.RegionIds.RightSide });

            return layoutRegions;
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
        public string Name { get; set; }
        public string RegionIdentifier { get; set; }
    }
    
    public class LayoutRegion : BaseModel
    {
        public Guid LayoutId { get; set; }
        public Guid RegionId { get; set; }
    }
}