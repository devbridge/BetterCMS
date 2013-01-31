using FluentMigrator.VersionTableInfo;

namespace BetterCms.Module.Templates.Models.Migrations
{
    [VersionTableMetaData]
    public class TemplatesVersionTableMetaData : IVersionTableMetaData
    {
        internal const string VersionInfoTableName = "VersionInfo";
        
        public string SchemaName
        {
            get
            {
                return "bcms_" + TemplatesModuleDescriptor.ModuleName;
            }
        }

        public string TableName
        {
            get
            {
                return VersionInfoTableName;
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
                return "uc_VersionInfo_Verion_" + TemplatesModuleDescriptor.ModuleName;
            }
        }
    }
}