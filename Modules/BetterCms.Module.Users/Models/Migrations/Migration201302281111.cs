using System;

using BetterCms.Core.DataAccess.DataContext.Migrations;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Models;

using FluentMigrator;

namespace BetterCms.Module.Users.Models.Migrations
{
    [Migration(201302281111)]
    public class Migration201302281111 : DefaultMigration
    {
        private readonly string rootSchemaName = (new Root.Models.Migrations.RootVersionTableMetaData()).SchemaName;
        private readonly string contentsTableName = "Contents";
        private readonly string widgetsTableName = "Widgets";

        private readonly string pagesSchemaName = (new Pages.Models.Migrations.PagesVersionTableMetaData()).SchemaName;
        private readonly string serverControlWidgetsTableName = "ServerControlWidgets";
        private readonly string contentOptionsTableName = "ContentOptions";

        public Migration201302281111() : base(UsersModuleDescriptor.ModuleName)
        {
        }

        public override void Up()
        {
            // Add widget.
            Insert.IntoTable(contentsTableName).InSchema(rootSchemaName).Row(new
                {
                    Id = UsersModuleConstants.LoginWidgetId,
                    Version = 1,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                    CreatedByUser = "Admin",
                    ModifiedOn = DateTime.Now,
                    ModifiedByUser = "Admin",
                    Name = "Login widget",
                    Status = (int)ContentStatus.Published
                });

            Insert.IntoTable(widgetsTableName).InSchema(rootSchemaName).Row(new
                {
                    Id = UsersModuleConstants.LoginWidgetId,
                });

            Insert.IntoTable(serverControlWidgetsTableName).InSchema(pagesSchemaName).Row(new
                {
                    Id = UsersModuleConstants.LoginWidgetId,
                    Url = "~/Areas/bcms-users/Views/Widgets/Login.cshtml"
                });

            // Add options.
            AddOption("LogInUrl", string.Empty);
            AddOption("LogOutUrl", string.Empty);
            AddOption("TitleText", "Account Information");
            AddOption("UserNameLabelText", "User name");
            AddOption("PasswordLabelText", "Password");
            AddOption("ShowFieldRememberMe", "true");
            AddOption("RememberMeLabelText", "Remember me?");
            AddOption("LogInButtonText", "Log On");
            AddOption("LogOutButtonText", "Log Off");
            AddOption("UserLoggedInText", "Welcome, {0}!");
            AddOption("MainDivCssClass", string.Empty);
        }

        private void AddOption(string optionName, string defaultValue)
        {
            var option = new
            {
                Id = Guid.NewGuid(),
                Version = 1,
                IsDeleted = false,
                CreatedOn = DateTime.Now,
                CreatedByUser = "Admin",
                ModifiedOn = DateTime.Now,
                ModifiedByUser = "Admin",
                ContentId = UsersModuleConstants.LoginWidgetId,
                Key = optionName,
                DefaultValue = defaultValue,
                Type = (int)OptionType.Text
            };

            Insert.IntoTable(contentOptionsTableName).InSchema(rootSchemaName).Row(option);
        }

        public override void Down()
        {
            Update.Table(contentsTableName)
                  .InSchema(rootSchemaName)
                  .Set(new { DeletedOn = DateTime.Now, DeletedByUser = "Admin" })
                  .Where(new { Id = UsersModuleConstants.LoginWidgetId });
        }
    }
}