using System;

using BetterCms.Core.DataAccess.DataContext.Migrations;
using BetterCms.Core.DataContracts.Enums;

using FluentMigrator;

namespace BetterCms.Module.Users.Models.Migrations
{
    /// <summary>
    /// Module database structure update.
    /// </summary>
    [Migration(201308210000)]
    public class Migration201308210000 : DefaultMigration
    {
        private readonly string rootSchemaName = (new Root.Models.Migrations.RootVersionTableMetaData()).SchemaName;
        private readonly string contentsTableName = "Contents";
        private readonly string widgetsTableName = "Widgets";

        private readonly string pagesSchemaName = (new Pages.Models.Migrations.PagesVersionTableMetaData()).SchemaName;
        private readonly string serverControlWidgetsTableName = "ServerControlWidgets";
        private readonly string contentOptionsTableName = "ContentOptions";

        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201308210000"/> class.
        /// </summary>
        public Migration201308210000()
            : base(UsersModuleDescriptor.ModuleName)
        {
        }

        /// <summary>
        /// Ups this instance.
        /// </summary>
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

        /// <summary>
        /// Adds the option.
        /// </summary>
        /// <param name="optionName">Name of the option.</param>
        /// <param name="defaultValue">The default value.</param>
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
    }
}