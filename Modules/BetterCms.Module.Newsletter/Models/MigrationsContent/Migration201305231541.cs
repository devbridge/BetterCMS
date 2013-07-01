using System.Collections.Generic;
using System.Transactions;

using BetterCms.Api;

using BetterCms.Core;
using BetterCms.Core.DataAccess.DataContext.Migrations;
using BetterCms.Core.DataContracts.Enums;

using BetterCms.Module.Pages.Api.DataContracts;
using BetterCms.Module.Pages.Api.DataContracts.Models;

using BetterCms.Module.Root.Models.MigrationsContent;

namespace BetterCms.Module.Newsletter.Models.MigrationsContent
{
    [ContentMigration(201305231541)]
    public class Migration201305231042 : ContentMigrationBase
    {
        public override void Up(ICmsConfiguration configuration)
        {
            using (var pagesApi = CmsContext.CreateApiContextOf<PagesApiContext>())
            {
                const string WidgetPath = "~/Areas/bcms-newsletter/Views/Widgets/SubscribeToNewsletter.cshtml";
                var request = new GetServerControlWidgetsRequest(e => e.Url == WidgetPath);
                var widgets = pagesApi.GetServerControlWidgets(request);
                if (widgets.Items.Count > 0)
                {
                    return;
                }

                using (var transactionScope = new TransactionScope())
                {
                    pagesApi.CreateServerControlWidget(
                    new CreateServerControlWidgetRequest()
                    {
                        Name = "Newsletter Widget",
                        WidgetPath = WidgetPath,
                        Options =
                            new List<ContentOptionDto>()
                                    {
                                        new ContentOptionDto() { Type = OptionType.Text, Key = "Email placeholder", DefaultValue = "email..." },
                                        new ContentOptionDto() { Type = OptionType.Text, Key = "Label title", DefaultValue = "Subscribe to newsletter" },
                                        new ContentOptionDto() { Type = OptionType.Text, Key = "Submit title", DefaultValue = "Submit" },
                                        new ContentOptionDto() { Type = OptionType.Text, Key = "Submit is disabled", DefaultValue = "false" }
                                    }
                    });
                    transactionScope.Complete();
                }
            }
        }
    }
}