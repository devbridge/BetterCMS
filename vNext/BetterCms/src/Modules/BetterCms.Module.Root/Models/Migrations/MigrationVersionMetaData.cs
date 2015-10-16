using FluentMigrator.VersionTableInfo;

namespace BetterCms.Module.Root.Models.Migrations
{
    [VersionTableMetaData]
    public class RootVersionTableMetaData : IVersionTableMetaData
    {
        public string SchemaName
        {
            get
            {
                return RootModuleDescriptor.RootSchemaName;
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
                return "uc_VersionInfo_Verion_" + RootModuleDescriptor.ModuleName;
            }
        }
    }
}