using FluentMigrator.VersionTableInfo;

namespace BetterCms.Module.Installation.Models.Migrations
{
    [VersionTableMetaData]
    public class InstallationVersionTableMetaData : IVersionTableMetaData
    {
        public string SchemaName
        {
            get
            {
                return InstallationModuleDescriptor.ModuleSchemaName;
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
                return "uc_VersionInfo_Verion_" + InstallationModuleDescriptor.ModuleName;
            }
        }
    }
}