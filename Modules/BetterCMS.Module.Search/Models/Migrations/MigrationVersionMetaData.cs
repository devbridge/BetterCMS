using FluentMigrator.VersionTableInfo;

namespace BetterCms.Module.Search.Models.Migrations
{
    [VersionTableMetaData]
    public class SearchVersionTableMetaData : IVersionTableMetaData
    {
        public string SchemaName
        {
            get
            {
                return SearchModuleDescriptor.SearchSchemaName;
            }
        }

        public string TableName
        {
            get
            {
                return "VersionInfo";
            }
        }

        public string ColumnName
        {
            get
            {
                return "Version";
            }
        }

        public string UniqueIndexName
        {
            get
            {
                return "uc_VersionInfo_Version_" + SearchModuleDescriptor.ModuleName;
            }
        }
    }
}