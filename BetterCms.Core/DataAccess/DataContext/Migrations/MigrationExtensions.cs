using BetterCms.Core.Models;

using FluentMigrator;
using FluentMigrator.Builders.Create.Table;

namespace BetterCms.Core.DataAccess.DataContext.Migrations
{
    public static class MigrationExtensions
    {
        public static ICreateTableWithColumnSyntax WithCmsBaseColumns(this ICreateTableWithColumnSyntax table)
        {
            return table
                .WithColumn("Id").AsGuid().PrimaryKey().WithDefault(SystemMethods.NewGuid)
                .WithColumn("Version").AsInt32().NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable().WithDefaultValue(false)
                .WithColumn("CreatedOn").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentDateTime)
                .WithColumn("CreatedByUser").AsString(MaxLength.Name).NotNullable()
                .WithColumn("ModifiedOn").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentDateTime)
                .WithColumn("ModifiedByUser").AsString(MaxLength.Name).NotNullable()
                .WithColumn("DeletedOn").AsDateTime().Nullable()
                .WithColumn("DeletedByUser").AsString(MaxLength.Name).Nullable();
        }
    }
}
