using FluentMigrator.VersionTableInfo;

namespace BetterCms.Module.LuceneSearch.Models.Migrations
{
    [VersionTableMetaData]
    public class MigrationVersionMetaData : IVersionTableMetaData
    {
        public string SchemaName
        {
            get
            {
                return LuceneSearchModuleDescriptor.LuceneSchemaName;
            }
        }

        public string TableName {
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
                return "uc_VersionInfo_Version_" + LuceneSearchModuleDescriptor.ModuleName;
            }
        }
    }
}