using FluentMigrator.VersionTableInfo;

namespace BetterCms.Module.Root.Models.Migrations
{
    [VersionTableMetaData]
    public class RootVersionTableMetaData : IVersionTableMetaData
    {
        public object ApplicationContext { get; set; }

        public bool OwnsSchema => true;

        public string SchemaName => RootModuleDescriptor.RootSchemaName;

        public string TableName => "VersionInfo";

        public string ColumnName => "Version";

        public string DescriptionColumnName => "Description";

        public string UniqueIndexName => "uc_VersionInfo_Verion_" + RootModuleDescriptor.ModuleName;

        public string AppliedOnColumnName => "AppliedOn";
    }
}