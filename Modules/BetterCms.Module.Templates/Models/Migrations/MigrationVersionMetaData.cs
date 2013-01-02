using FluentMigrator.VersionTableInfo;

namespace BetterCms.Module.Templates.Models.Migrations
{
    [VersionTableMetaData]
    public class TemplatesVersionTableMetaData : IVersionTableMetaData
    {
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
                return "uc_VersionInfo_Verion_" + TemplatesModuleDescriptor.ModuleName;
            }
        }
    }
}