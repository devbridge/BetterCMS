using System;

using BetterModules.Core.DataAccess.DataContext.Migrations;

using FluentMigrator;

namespace BetterCms.Module.Installation.Models.Migrations
{
    /// <summary>
    /// Module database structure update.
    /// </summary>
    [Migration(201511041142)]
    public class Migration201511041142 : DefaultMigration
    {
        /// <summary>
        /// The root schema name
        /// </summary>
        private readonly string rootSchemaName;

        /// <summary>
        /// The pages schema name
        /// </summary>
        private readonly string pagesSchemaName;

        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201511041142"/> class.
        /// </summary>
        public Migration201511041142()
            : base(InstallationModuleDescriptor.ModuleName)
        {
            rootSchemaName = (new Root.Models.Migrations.RootVersionTableMetaData()).SchemaName;
            pagesSchemaName = (new Pages.Models.Migrations.PagesVersionTableMetaData()).SchemaName;
        }

        /// <summary>
        /// Ups this instance.
        /// </summary>
        public override void Up()
        {
            CreateDisqusWidget();
        }

        /// <summary>
        /// Creates the widget
        /// </summary>
        private void CreateDisqusWidget()
        {
            var widget = new
            {
                ForRootSchemaContentTable = new
                {
                    Id = "2DFA000C-5FFE-45FF-98C9-320A942D86EF",
                    Version = 1,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                    CreatedByUser = "Better CMS",
                    ModifiedOn = DateTime.Now,
                    ModifiedByUser = "Better CMS",
                    Name = "Disqus Widget",
                    Status = 3,
                    PublishedOn = DateTime.Now,
                    PublishedByUser = "Better CMS"
                },

                ForRootScemaWidgetsTable = new
                {
                    Id = "2DFA000C-5FFE-45FF-98C9-320A942D86EF"
                },

                ForPagesSchemaHtmlContentWidgetsTable = new 
                {
                    Id = "2DFA000C-5FFE-45FF-98C9-320A942D86EF",
                    UseCustomCss = false,
                    UseCustomJs = false,
                    UseHtml = true,
                    EditInSourceMode = true,
                    Html = 
@"<div id=""disqus_thread""></div>

<script>
    var disqus_shortname = '{{CmsWidgetOption:DisqusShortName}}';
    var disqus_title = '{{CmsPageTitle}}';
    var disqus_category_id = '{{CmsWidgetOption:DisqusCategoryId}}' ? '{{CmsWidgetOption:DisqusCategoryId}}' : undefined;

    var disqus_config = function () {  
        this.page.identifier = '{{CmsPageId}}';
        this.page.title = 
        this.page.category_id = '{{CmsPageOption:DisqusCategoryId}}' ? '{{CmsPageOption:DisqusCategoryId}}' : undefined;
    };
    
    (function() {  // DON'T EDIT BELOW THIS LINE
        var d = document, s = d.createElement('script');
    
        s.src = '//'+ disqus_shortname +'.disqus.com/embed.js';
    
        s.setAttribute('data-timestamp', +new Date());
        (d.head || d.body).appendChild(s);
    })();
</script>
<noscript>Please enable JavaScript to view the <a href=""https://disqus.com/?ref_noscript"" rel=""nofollow"">comments powered by Disqus.</a></noscript>"
                }
            };

            // Register server control widget.
            Insert.IntoTable("Contents").InSchema(rootSchemaName).Row(widget.ForRootSchemaContentTable);
            Insert.IntoTable("Widgets").InSchema(rootSchemaName).Row(widget.ForRootScemaWidgetsTable);
            Insert.IntoTable("HtmlContentWidgets").InSchema(pagesSchemaName).Row(widget.ForPagesSchemaHtmlContentWidgetsTable);
        }
    }
}